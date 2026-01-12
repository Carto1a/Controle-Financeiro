using ControleFinanceiro.Domain.Transacoes;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.EntityFramework.Repositories;

public class TransacaoRepository(AppDbContext context) : ITransacaoRepository
{
    private readonly AppDbContext _context = context;

    public void Criar(Transacao transacao)
    {
        _context.Transacaos.Add(transacao);
        _context.Transacaos
            .Entry(transacao)
            .Property("TipoTransacaoId")
            .CurrentValue = transacao.TipoTransacao;
    }

    public async Task DeletarPorPessoaIdAsync(Guid pessoaId, CancellationToken cancellationToken = default)
    {
        var transacoes = await _context.Transacaos
            .Where(x => EF.Property<Guid>(x, "PessoaId") == pessoaId)
            .ToListAsync(cancellationToken);

        _context.Transacaos.RemoveRange(transacoes);
    }
}
