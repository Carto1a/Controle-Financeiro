namespace ControleFinanceiro.Domain.Transacoes;

public interface ICategoriaRepository
{
    void Criar(Categoria categoria);
    Task<Categoria?> BuscarPorIdAsync(Guid id, CancellationToken cancellationToken = default);
}
