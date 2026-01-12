using ControleFinanceiro.Application.DataLayer;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record ObterPessoaReponse(Guid Id, string Nome, DateTime DataNascimento);

public class ObterPessoaQueryHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<ObterPessoaReponse?>> Handle(Guid id, CancellationToken cancellationToken = default) =>
        await _queries.ObterPessoaPorId(id, cancellationToken);
}
