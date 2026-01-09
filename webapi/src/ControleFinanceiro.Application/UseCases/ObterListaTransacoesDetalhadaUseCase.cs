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

public class ObterListaTransacoesDetalhadaQueryHandler(IQueries queries)
{
    private readonly IQueries _queries = queries;

    public async Task<Result<List<ObterListaTransacoesDetalhadaResponse>>> Handle()
        => await _queries.ObterListaTransacoesDetalhada();
}
