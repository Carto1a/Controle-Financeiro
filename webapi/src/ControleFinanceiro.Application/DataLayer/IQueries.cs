using ControleFinanceiro.Application.Abstractions.Data;
using ControleFinanceiro.Application.UseCases;

namespace ControleFinanceiro.Application.DataLayer;

public interface IQueries
{
    Task<List<ObterListaCategoriasBasicaResponse>> ObterListaCategoriasBasica(CancellationToken cancellationToken = default);
    Task<PaginatedResponse<ObterListaCategoriasDetalhadaResponse>> ObterListaCategoriasDetalhada(
        ObterListaCategoriasDetalhadaQuery request,
        CancellationToken cancellationToken = default);
    Task<ObterCategoriaReponse?> ObterCategoriaPorId(Guid id, CancellationToken cancellationToken = default);

    Task<List<ObterListaPessoasBasicaResponse>> ObterListaPessoasBasica(CancellationToken cancellationToken = default);
    Task<PaginatedResponse<ObterListaPessoasDetalhadaResponse>> ObterListaPessoasDetalhada(
        ObterListaPessoasDetalhadaQuery request,
        CancellationToken cancellationToken = default);
    Task<ObterPessoaReponse?> ObterPessoaPorId(Guid id, CancellationToken cancellationToken = default);

    Task<PaginatedResponse<ObterListaTransacoesDetalhadaResponse>> ObterListaTransacoesDetalhada(
        ObterListaTransacoesDetalhadaQuery request,
        CancellationToken cancellationToken = default);

    Task<PaginatedResponse<ResumoFinanceiroResponse>> ObterResumoFinanceiroPorCategoria(
        ObterResumoFinanceiroPaginatedQuery request,
        CancellationToken cancellationToken = default);
    Task<PaginatedResponse<ResumoFinanceiroResponse>> ObterResumoFinanceiroPorPessoa(
        ObterResumoFinanceiroPaginatedQuery request,
        CancellationToken cancellationToken = default);
    Task<ResumoFinanceiroValoresResponse> ObterResumoFinanceiroTotal(CancellationToken cancellationToken = default);
}
