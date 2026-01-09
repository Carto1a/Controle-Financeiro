namespace ControleFinanceiro.Domain.Transacoes;

public interface ITransacaoRepository
{
    void Criar(Transacao transacao);
    Task DeletarPorPessoaIdAsync(Guid pessoaId, CancellationToken cancellationToken = default);
}
