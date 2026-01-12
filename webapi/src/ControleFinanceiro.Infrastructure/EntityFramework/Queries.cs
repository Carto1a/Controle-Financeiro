using ControleFinanceiro.Application.Abstractions.Data;
using ControleFinanceiro.Application.DataLayer;
using ControleFinanceiro.Application.UseCases;
using ControleFinanceiro.Domain.Transacoes;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.EntityFramework;

public class Queries(AppDbContext context) : IQueries
{
    private readonly AppDbContext _context = context;

    public Task<List<ObterListaCategoriasBasicaResponse>> ObterListaCategoriasBasica(CancellationToken cancellationToken = default)
    {
        return _context.Categorias
            .AsNoTracking()
            .Select(x => new ObterListaCategoriasBasicaResponse(x.Id, x.Nome))
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedResponse<ObterListaCategoriasDetalhadaResponse>> ObterListaCategoriasDetalhada(
        ObterListaCategoriasDetalhadaQuery request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Categorias
            .AsNoTracking()
            .Select(x => new { x.Id, x.Nome, x.Descricao, Finalidade = EF.Property<Finalidade>(x, "FinalidadeId") });

        var total = await query.CountAsync();
        var paged = await query
            .OrderBy(x => x.Descricao)
            .Skip(request.Pagina * request.TamanhoPagina)
            .Take(request.TamanhoPagina)
            .ToListAsync(cancellationToken);

        var items = paged.Select(x => new ObterListaCategoriasDetalhadaResponse(x.Id, x.Nome, x.Descricao, x.Finalidade)).ToList();

        return new PaginatedResponse<ObterListaCategoriasDetalhadaResponse>(request, items, total);
    }

    public Task<List<ObterListaPessoasBasicaResponse>> ObterListaPessoasBasica(CancellationToken cancellationToken = default)
    {
        return _context.Pessoas
            .AsNoTracking()
            .Select(x => new ObterListaPessoasBasicaResponse(x.Id, x.Nome))
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedResponse<ObterListaPessoasDetalhadaResponse>> ObterListaPessoasDetalhada(
        ObterListaPessoasDetalhadaQuery request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Pessoas
            .AsNoTracking()
            .Select(x => new { x.Id, x.Nome });

        var total = await query.CountAsync();
        var paged = await query
            .OrderBy(x => x.Nome)
            .Skip(request.Pagina * request.TamanhoPagina)
            .Take(request.TamanhoPagina)
            .ToListAsync(cancellationToken);

        var items = paged.Select(x => new ObterListaPessoasDetalhadaResponse(x.Id, x.Nome)).ToList();

        return new PaginatedResponse<ObterListaPessoasDetalhadaResponse>(request, items, total);
    }

    public async Task<PaginatedResponse<ObterListaTransacoesDetalhadaResponse>> ObterListaTransacoesDetalhada(
        ObterListaTransacoesDetalhadaQuery request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Transacaos
            .AsNoTracking()
            .LeftJoin(
                _context.Categorias,
                transacao => transacao.CategoriaId,
                categoria => categoria.Id,
                (transacao, categoria) => new
                {
                    transacao.Id,
                    PessoaId = EF.Property<Guid>(transacao, "PessoaId"),
                    transacao.Descricao,
                    TipoTransacao = EF.Property<TipoTransacao>(transacao, "TipoTransacaoId"),
                    transacao.Valor,
                    transacao.Data,
                    transacao.CriadoEm,
                    Categoria = categoria
                })
            .LeftJoin(
                _context.Pessoas,
                transacao => transacao.PessoaId,
                pessoa => pessoa.Id,
                (transacao, pessoa) => new
                {
                    transacao.Id,
                    transacao.Descricao,
                    transacao.TipoTransacao,
                    transacao.Valor,
                    transacao.Data,
                    transacao.CriadoEm,
                    transacao.Categoria,
                    Pessoa = pessoa
                })
            .Select(x => new
            {
                x.Id,
                x.Descricao,
                x.Valor,
                x.TipoTransacao,
                CategoriaId = x.Categoria.Id,
                CategoriaNome = x.Categoria.Nome,
                PessoaId = x.Pessoa.Id,
                PessoaNome = x.Pessoa.Nome,
                x.Data,
                x.CriadoEm
            });

        var total = await query.CountAsync();
        var paged = await query
            .OrderByDescending(x => x.Data)
            .Skip(request.Pagina * request.TamanhoPagina)
            .Take(request.TamanhoPagina)
            .ToListAsync(cancellationToken);

        var items = paged
            .Select(x => new ObterListaTransacoesDetalhadaResponse(
                x.Id,
                x.Descricao,
                x.Valor,
                x.TipoTransacao,
                new ObterListaCategoriasBasicaResponse(x.CategoriaId, x.CategoriaNome),
                new ObterListaPessoasBasicaResponse(x.PessoaId, x.PessoaNome),
                x.Data,
                x.CriadoEm))
            .ToList();

        return new PaginatedResponse<ObterListaTransacoesDetalhadaResponse>(request, items, total);
    }

    public async Task<PaginatedResponse<ResumoFinanceiroResponse>> ObterResumoFinanceiroPorCategoria(
        ObterResumoFinanceiroPaginatedQuery request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Categorias
            .AsNoTracking()
            .LeftJoin(
                _context.Transacaos,
                categoria => categoria.Id,
                transacao => transacao.CategoriaId,
                (categoria, transacao) => new { Transacao = transacao, Categoria = categoria })
            .GroupBy(x => new { x.Categoria.Id, x.Categoria.Nome })
            .Select(x => new
            {
                x.Key.Id,
                x.Key.Nome,
                TotalDespesas = x
                    .Where(x => x.Transacao != null && EF.Property<TipoTransacao>(x.Transacao, "TipoTransacaoId") == TipoTransacao.Despesa)
                    .Sum(x => x.Transacao != null ? x.Transacao.Valor : 0),
                TotalReceitas = x
                    .Where(x => x.Transacao != null && EF.Property<TipoTransacao>(x.Transacao, "TipoTransacaoId") == TipoTransacao.Receita)
                    .Sum(x => x.Transacao != null ? x.Transacao.Valor : 0),
            });

        var total = await query.CountAsync();
        var paged = await query
            .OrderBy(x => x.Nome)
            .Skip(request.Pagina * request.TamanhoPagina)
            .Take(request.TamanhoPagina)
            .ToListAsync(cancellationToken);

        var items = paged
            .Select(x =>
                new ResumoFinanceiroResponse()
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    TotalDespesas = x.TotalDespesas,
                    TotalReceitas = x.TotalReceitas
                })
            .ToList();

        return new PaginatedResponse<ResumoFinanceiroResponse>(request, items, total);
    }

    public async Task<PaginatedResponse<ResumoFinanceiroResponse>> ObterResumoFinanceiroPorPessoa(
        ObterResumoFinanceiroPaginatedQuery request,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Pessoas
            .AsNoTracking()
            .LeftJoin(
                _context.Transacaos,
                pessoa => pessoa.Id,
                transacao => transacao.CategoriaId,
                (pessoa, transacao) => new { Transacao = transacao, Pessoa = pessoa })
            .GroupBy(x => new { x.Pessoa.Id, x.Pessoa.Nome })
            .Select(x => new
            {
                x.Key.Id,
                x.Key.Nome,
                TotalDespesas = x
                    .Where(x => x.Transacao != null && EF.Property<TipoTransacao>(x.Transacao, "TipoTransacaoId") == TipoTransacao.Despesa)
                    .Sum(x => x.Transacao != null ? x.Transacao.Valor : 0),
                TotalReceitas = x
                    .Where(x => x.Transacao != null && EF.Property<TipoTransacao>(x.Transacao, "TipoTransacaoId") == TipoTransacao.Receita)
                    .Sum(x => x.Transacao != null ? x.Transacao.Valor : 0),
            });

        var total = await query.CountAsync();
        var paged = await query
            .OrderBy(x => x.Nome)
            .Skip(request.Pagina * request.TamanhoPagina)
            .Take(request.TamanhoPagina)
            .ToListAsync(cancellationToken);

        var items = paged
            .Select(x =>
                new ResumoFinanceiroResponse()
                {
                    Id = x.Id,
                    Nome = x.Nome,
                    TotalDespesas = x.TotalDespesas,
                    TotalReceitas = x.TotalReceitas
                })
            .ToList();

        return new PaginatedResponse<ResumoFinanceiroResponse>(request, items, total);
    }

    public Task<ResumoFinanceiroValoresResponse> ObterResumoFinanceiroTotal(CancellationToken cancellationToken = default)
    {
        return _context.Transacaos
            .AsNoTracking()
            .GroupBy(x => 1)
            .Select(x => new ResumoFinanceiroValoresResponse()
            {
                TotalDespesas = x
                    .Where(x => EF.Property<TipoTransacao>(x, "TipoTransacaoId") == TipoTransacao.Despesa)
                    .Sum(x => x.Valor),
                TotalReceitas = x
                    .Where(x => EF.Property<TipoTransacao>(x, "TipoTransacaoId") == TipoTransacao.Receita)
                    .Sum(x => x.Valor),
            })
            .FirstAsync(cancellationToken);
    }
}
