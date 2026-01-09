using ControleFinanceiro.Domain.Pessoas;
using ControleFinanceiro.Domain.Transacoes;

namespace ControleFinanceiro.Domain.EventHandlers;

public class RemoverTransacoesAoDeletarPessoaEventHandler(ITransacaoRepository transacaoRepository) : IDomainEventHandler<PessoaDeletadaEvent>
{
    private readonly ITransacaoRepository _transacaoRepository = transacaoRepository;

    public Task Handle(PessoaDeletadaEvent evt, CancellationToken cancellationToken = default)
    {
        return _transacaoRepository.DeletarPorPessoaIdAsync(evt.PessoaId, cancellationToken);
    }
}
