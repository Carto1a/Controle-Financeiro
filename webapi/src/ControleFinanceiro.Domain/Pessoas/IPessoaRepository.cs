namespace ControleFinanceiro.Domain.Pessoas;

/// <summary>
/// Interface de repositório para o aggregate root Pessoa.
/// Define operações básicas de persistência no domínio.
/// </summary>
public interface IPessoaRepository
{
    void Criar(Pessoa pessoa);
    void Deletar(Pessoa pessoa);
    Task<Pessoa?> BuscarPorIdAsync(Guid id, CancellationToken cancellationToken = default);
}
