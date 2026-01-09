using ControleFinanceiro.Application.UseCases;

namespace ControleFinanceiro.Application.DataLayer;

public interface IQueries
{
    Task<List<ObterListaCategoriasBasicaResponse>> ObterListaCategoriasBasica(CancellationToken cancellationToken = default);
    Task<List<ObterListaCategoriasDetalhadaResponse>> ObterListaCategoriasDetalhada(CancellationToken cancellationToken = default);

    Task<List<ObterListaPessoasBasicaResponse>> ObterListaPessoasBasica(CancellationToken cancellationToken = default);
    Task<List<ObterListaPessoasDetalhadaResponse>> ObterListaPessoasDetalhada(CancellationToken cancellationToken = default);

    Task<List<ObterListaTransacoesDetalhadaResponse>> ObterListaTransacoesDetalhada(CancellationToken cancellationToken = default);
}
