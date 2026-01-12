using ControleFinanceiro.Application.Abstractions.Data;
using ControleFinanceiro.Application.DataLayer;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public class ObterResumoFinanceiroPorPessoaQueryHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<PaginatedResponse<ResumoFinanceiroResponse>>> Handle(
        ObterResumoFinanceiroPaginatedQuery request,
        CancellationToken cancellationToken = default)
    {
        var resultPagination = Pagination.Validate(request);
        if (resultPagination.IsFailed) return Result.Fail(resultPagination.Errors);

        return await _queries.ObterResumoFinanceiroPorPessoa(request, cancellationToken);
    }
}
