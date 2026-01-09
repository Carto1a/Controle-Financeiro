using ControleFinanceiro.Application.UseCases;

namespace ControleFinanceiro.Application.DataLayer;

public interface IQueries
{
    Task<ObterListaCategoriasBasicaResponse> ObterListaCategoriasBasica(CancellationToken cancellationToken = default);
    Task<ObterListaCategoriasDetalhadaResponse> ObterListaCategoriasDetalhada(CancellationToken cancellationToken = default);

    Task<ObterListaPessoasBasicaResponse> ObterListaPessoasBasica(CancellationToken cancellationToken = default);
    Task<ObterListaPessoasDetalhadaResponse> ObterListaPessoasDetalhada(CancellationToken cancellationToken = default);

    Task<ObterListaTransacoesDetalhadaResponse> ObterListaTransacoesDetalhada(CancellationToken cancellationToken = default);
}
