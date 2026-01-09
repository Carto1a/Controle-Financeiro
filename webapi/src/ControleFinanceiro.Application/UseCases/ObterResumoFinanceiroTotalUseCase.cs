using ControleFinanceiro.Application.DataLayer;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record ObterResumoFinanceiroTotalResponse(Guid Id, string Nome);

public class ObterResumoFinanceiroTotalQueryHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<ResumoFinanceiroValoresResponse>> Handle(CancellationToken cancellationToken = default)
        => await _queries.ObterResumoFinanceiroTotal(cancellationToken);
}
