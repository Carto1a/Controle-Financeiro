namespace ControleFinanceiro.Domain.Transacoes;

public interface ITransacaoRepository
{
    void Criar(Transacao transacao);
    void DeletarPorPessoaId(Guid pessoaId);
}
