using ControleFinanceiro.Application.DataLayer;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public class ObterResumoFinanceiroPorCategoriaCommandHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<List<ResumoFinanceiroResponse>>> Handle()
        => await _queries.ObterResumoFinanceiroPorCategoria();
}
