using ControleFinanceiro.Application.DataLayer;

namespace ControleFinanceiro.Infrastructure.EntityFramework;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
