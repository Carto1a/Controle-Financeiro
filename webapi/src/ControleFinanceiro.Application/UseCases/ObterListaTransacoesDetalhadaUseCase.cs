using ControleFinanceiro.Application.Abstractions.Data;
using ControleFinanceiro.Application.DataLayer;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record ObterListaTransacoesDetalhadaResponse(
    Guid Id,
    string Descricao,
    decimal Valor,
    string TipoTransacao,
    string Categoria,
    DateTime Data,
    DateTime CriadoEm);

public record ObterListaTransacoesDetalhadaQuery(int Pagina, int TamanhoPagina) : IPagination;

public class ObterListaTransacoesDetalhadaQueryHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<PaginatedResponse<ObterListaTransacoesDetalhadaResponse>>> Handle(
        ObterListaTransacoesDetalhadaQuery request,
        CancellationToken cancellationToken = default)
    {
        var resultPagination = Pagination.Validate(request);
        if (resultPagination.IsFailed) return Result.Fail(resultPagination.Errors);

        return await _queries.ObterListaTransacoesDetalhada(request, cancellationToken);
    }
}
