using ControleFinanceiro.Application.DataLayer;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record ObterResumoFinanceiroTotalResponse(Guid Id, string Nome);

public class ObterResumoFinanceiroTotalCommandHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<ResumoFinanceiroValoresResponse>> Handle()
        => await _queries.ObterResumoFinanceiroTotal();
}
