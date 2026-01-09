using ControleFinanceiro.Application.DataLayer;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record ObterListaPessoasBasicaResponse(Guid Id, string Nome);

public class ObterListaPessoasBasicaQueryHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<List<ObterListaPessoasBasicaResponse>>> Handle(CancellationToken cancellationToken = default)
        => await _queries.ObterListaPessoasBasica(cancellationToken);
}
