using ControleFinanceiro.Application.DataLayer;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record ObterListaPessoasDetalhadaResponse(Guid Id, string Nome);

public class ObterListaPessoasDetalhadaQueryHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<List<ObterListaPessoasDetalhadaResponse>>> Handle(CancellationToken cancellationToken)
        => await _queries.ObterListaPessoasDetalhada(cancellationToken);
}
