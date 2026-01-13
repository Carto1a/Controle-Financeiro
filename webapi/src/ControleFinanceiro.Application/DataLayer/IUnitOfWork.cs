namespace ControleFinanceiro.Application.DataLayer;

/// <summary>
/// Interface para Unit of Work.
///
/// Permite agrupar operações de persistência em uma única transação,
/// garantindo consistência dos dados.
/// </summary>
public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken cancellationToken = default);
}
