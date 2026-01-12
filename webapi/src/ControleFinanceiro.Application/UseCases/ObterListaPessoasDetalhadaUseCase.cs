using ControleFinanceiro.Application.Abstractions.Data;
using ControleFinanceiro.Application.DataLayer;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record ObterListaPessoasDetalhadaResponse(Guid Id, string Nome, DateTime DataNascimento);
public record ObterListaPessoasDetalhadaQuery(int Pagina, int TamanhoPagina) : IPagination;

public class ObterListaPessoasDetalhadaQueryHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<PaginatedResponse<ObterListaPessoasDetalhadaResponse>>> Handle(
        ObterListaPessoasDetalhadaQuery request,
        CancellationToken cancellationToken)
    {
        var resultPagination = Pagination.Validate(request);
        if (resultPagination.IsFailed) return Result.Fail(resultPagination.Errors);

        return await _queries.ObterListaPessoasDetalhada(request, cancellationToken);
    }
}
