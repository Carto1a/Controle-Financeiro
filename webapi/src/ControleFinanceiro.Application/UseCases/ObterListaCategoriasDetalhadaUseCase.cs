using ControleFinanceiro.Application.DataLayer;
using ControleFinanceiro.Domain.Transacoes;
using FluentResults;

namespace ControleFinanceiro.Application.UseCases;

public record ObterListaCategoriasDetalhadaResponse(Guid Id, string Descricao, Finalidade Finalidade);

public class ObterListaCategoriasDetalhadaCommandHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<List<ObterListaCategoriasDetalhadaResponse>>> Handle()
        => await _queries.ObterListaCategoriasDetalhada();
}
