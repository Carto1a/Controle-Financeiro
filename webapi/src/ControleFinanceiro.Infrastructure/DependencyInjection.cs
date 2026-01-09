using ControleFinanceiro.Application.DataLayer;
using ControleFinanceiro.Application.UseCases;
using ControleFinanceiro.Domain;
using ControleFinanceiro.Domain.EventHandlers;
using ControleFinanceiro.Domain.Pessoas;
using ControleFinanceiro.Domain.Transacoes;
using ControleFinanceiro.Infrastructure.EntityFramework;
using ControleFinanceiro.Infrastructure.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection InjectDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Database")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPessoaRepository, PessoaRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<ITransacaoRepository, TransacaoRepository>();
        services.AddScoped<IQueries, Queries>();

        services.AddScoped<IDomainEventHandler<PessoaDeletadaEvent>, RemoverTransacoesAoDeletarPessoaEventHandler>();

        services.AddScoped<CriarCategoriaCommandHandler>();
        services.AddScoped<CriarPessoaCommandHandler>();
        services.AddScoped<CriarTransacaoCommandHandler>();
        services.AddScoped<DeletarPessoaCommandHandler>();
        services.AddScoped<ObterListaCategoriasBasicaQueryHandler>();
        services.AddScoped<ObterListaCategoriasDetalhadaQueryHandler>();
        services.AddScoped<ObterListaPessoasBasicaQueryHandler>();
        services.AddScoped<ObterListaPessoasDetalhadaQueryHandler>();
        services.AddScoped<ObterListaTransacoesDetalhadaQueryHandler>();
        services.AddScoped<ObterResumoFinanceiroPorCategoriaQueryHandler>();
        services.AddScoped<ObterResumoFinanceiroPorPessoaQueryHandler>();
        services.AddScoped<ObterResumoFinanceiroTotalQueryHandler>();

        return services;
    }
}
