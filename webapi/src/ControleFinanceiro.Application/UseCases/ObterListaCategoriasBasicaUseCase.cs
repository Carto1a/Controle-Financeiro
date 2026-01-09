using ControleFinanceiro.Application.DataLayer;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record ObterListaCategoriasBasicaResponse(Guid Id, string Descricao);

public class ObterListaCategoriasBasicaQueryHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<List<ObterListaCategoriasBasicaResponse>>> Handle(CancellationToken cancellationToken = default)
        => await _queries.ObterListaCategoriasBasica(cancellationToken);
}
