namespace ControleFinanceiro.Domain.Transacoes;

/// <summary>
/// Interface de repositório para o aggregate root Transação.
/// Define operações básicas de persistência no domínio.
/// </summary>
public interface ITransacaoRepository
{
    void Criar(Transacao transacao);
    Task DeletarPorPessoaIdAsync(Guid pessoaId, CancellationToken cancellationToken = default);
}
