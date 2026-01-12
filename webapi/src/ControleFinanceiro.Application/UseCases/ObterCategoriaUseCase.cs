using ControleFinanceiro.Application.DataLayer;
using ControleFinanceiro.Domain.Transacoes;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record ObterCategoriaReponse(Guid Id, string Nome, string Descricao, Finalidade Finalidade);

public class ObterCategoriaQueryHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<ObterCategoriaReponse?>> Handle(Guid id, CancellationToken cancellationToken = default) =>
        await _queries.ObterCategoriaPorId(id, cancellationToken);
}
