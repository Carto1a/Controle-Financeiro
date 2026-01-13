namespace ControleFinanceiro.Domain.Transacoes;

/// <summary>
/// Interface de repositório para o aggregate Categoria.
/// Define operações básicas de persistência no domínio.
/// </summary>
public interface ICategoriaRepository
{
    void Criar(Categoria categoria);
    Task<Categoria?> BuscarPorIdAsync(Guid id, CancellationToken cancellationToken = default);
}
