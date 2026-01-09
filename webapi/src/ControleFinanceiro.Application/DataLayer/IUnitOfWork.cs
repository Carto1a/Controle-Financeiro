namespace ControleFinanceiro.Application.DataLayer;

public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken cancellationToken = default);
}
