using ControleFinanceiro.Domain;
using ControleFinanceiro.Domain.Pessoas;
using ControleFinanceiro.Domain.Transacoes;
using ControleFinanceiro.Infrastructure.EntityFramework.Mappings;
using ControleFinanceiro.Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.EntityFramework;

public class AppDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;

    public virtual DbSet<Pessoa> Pessoas { get; set; }
    public virtual DbSet<Transacao> Transacaos { get; set; }
    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<TipoTransacaoModel> TipoTransacoes { get; set; }
    public virtual DbSet<FinalidadeModel> Finalidades { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options, IServiceProvider serviceProvider)
        : base(options)
    {
        _serviceProvider = serviceProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(PessoaMapping).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await PublishDomainEvents(cancellationToken);
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }

    private async Task PublishDomainEvents(CancellationToken cancellationToken)
    {
        var domainEvents = ChangeTracker
            .Entries<AggregateRoot>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IList<IDomainEvent> domainEvents = (IList<IDomainEvent>)entity.DomainEvents;

                entity.ClearEvents();

                return domainEvents;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>)
                .MakeGenericType(domainEvent.GetType());

            var handlers = _serviceProvider.GetServices(handlerType);

            foreach (var handler in handlers)
            {
                if (handler is null) throw new NullReferenceException("event handler null");

                await (Task)handlerType
                    .GetMethod(nameof(IDomainEventHandler<IDomainEvent>.Handle))!
                    .Invoke(handler, [domainEvent, cancellationToken])!;
            }
        }
    }
}
