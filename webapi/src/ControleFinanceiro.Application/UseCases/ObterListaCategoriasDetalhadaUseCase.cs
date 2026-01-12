using ControleFinanceiro.Application.Abstractions.Data;
using ControleFinanceiro.Application.DataLayer;
using ControleFinanceiro.Domain.Transacoes;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record ObterListaCategoriasDetalhadaResponse(Guid Id, string Nome, string Descricao, Finalidade Finalidade);
public record ObterListaCategoriasDetalhadaQuery(int Pagina, int TamanhoPagina) : IPagination;

public class ObterListaCategoriasDetalhadaQueryHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<PaginatedResponse<ObterListaCategoriasDetalhadaResponse>>> Handle(
        ObterListaCategoriasDetalhadaQuery request,
        CancellationToken cancellationToken = default)
    {
        var resultPagination = Pagination.Validate(request);
        if (resultPagination.IsFailed) return Result.Fail(resultPagination.Errors);

        return await _queries.ObterListaCategoriasDetalhada(request, cancellationToken);
    }
}
