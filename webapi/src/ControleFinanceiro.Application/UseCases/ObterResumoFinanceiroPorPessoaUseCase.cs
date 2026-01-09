using ControleFinanceiro.Application.DataLayer;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public class ObterResumoFinanceiroPorPessoaQueryHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<List<ResumoFinanceiroResponse>>> Handle(CancellationToken cancellationToken = default)
        => await _queries.ObterResumoFinanceiroPorPessoa(cancellationToken);
}
