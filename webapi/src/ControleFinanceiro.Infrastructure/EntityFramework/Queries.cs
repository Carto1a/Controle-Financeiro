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
            .Select(x => new ObterListaCategoriasBasicaResponse(x.Id, x.Descricao))
            .ToListAsync(cancellationToken);
    }

    public Task<List<ObterListaCategoriasDetalhadaResponse>> ObterListaCategoriasDetalhada(CancellationToken cancellationToken = default)
    {
        return _context.Categorias
            .Select(x => new ObterListaCategoriasDetalhadaResponse(x.Id, x.Descricao, x.Finalidade))
            .ToListAsync(cancellationToken);
    }

    public Task<List<ObterListaPessoasBasicaResponse>> ObterListaPessoasBasica(CancellationToken cancellationToken = default)
    {
        return _context.Pessoas
            .Select(x => new ObterListaPessoasBasicaResponse(x.Id, x.Nome))
            .ToListAsync(cancellationToken);
    }

    public Task<List<ObterListaPessoasDetalhadaResponse>> ObterListaPessoasDetalhada(CancellationToken cancellationToken = default)
    {
        return _context.Pessoas
            .Select(x => new ObterListaPessoasDetalhadaResponse(x.Id, x.Nome))
            .ToListAsync(cancellationToken);
    }

    public Task<List<ObterListaTransacoesDetalhadaResponse>> ObterListaTransacoesDetalhada(CancellationToken cancellationToken = default)
    {
        return _context.Transacaos
            .LeftJoin(
                _context.Categorias,
                transacao => transacao.CategoriaId,
                categoria => categoria.Id,
                (transacao, categoria) => new
                {
                    transacao.Id,
                    transacao.Descricao,
                    transacao.TipoTransacao,
                    transacao.Valor,
                    transacao.Data,
                    transacao.CriadoEm,
                    Categoria = categoria != null ? categoria.Nome : "sem nome"
                })
            .Select(x => new ObterListaTransacoesDetalhadaResponse(
                x.Id,
                x.Descricao,
                x.Valor,
                x.TipoTransacao.ToString(),
                x.Categoria ?? "sem nome",
                x.Data,
                x.CriadoEm))
            .ToListAsync(cancellationToken);
    }

    public Task<List<ResumoFinanceiroResponse>> ObterResumoFinanceiroPorCategoria(CancellationToken cancellationToken = default)
    {
        return _context.Categorias
            .LeftJoin(
                _context.Transacaos,
                categoria => categoria.Id,
                transacao => transacao.CategoriaId,
                (categoria, transacao) => new { Transacao = transacao, Categoria = categoria })
            .GroupBy(x => new { x.Categoria.Id, x.Categoria.Nome })
            .Select(x => new ResumoFinanceiroResponse()
            {
                Id = x.Key.Id,
                Nome = x.Key.Nome,
                TotalDespesas = x
                    .Where(x => x.Transacao != null && x.Transacao.TipoTransacao == TipoTransacao.Despesa)
                    .Sum(x => x.Transacao != null? x.Transacao.Valor : 0),
                TotalReceitas = x
                    .Where(x => x.Transacao != null && x.Transacao.TipoTransacao == TipoTransacao.Receita)
                    .Sum(x => x.Transacao != null? x.Transacao.Valor : 0),
            })
            .ToListAsync(cancellationToken);
    }

    public Task<List<ResumoFinanceiroResponse>> ObterResumoFinanceiroPorPessoa(CancellationToken cancellationToken = default)
    {
        return _context.Pessoas
            .LeftJoin(
                _context.Transacaos,
                pessoa => pessoa.Id,
                transacao => transacao.CategoriaId,
                (pessoa, transacao) => new { Transacao = transacao, Pessoa = pessoa })
            .GroupBy(x => new { x.Pessoa.Id, x.Pessoa.Nome })
            .Select(x => new ResumoFinanceiroResponse()
            {
                Id = x.Key.Id,
                Nome = x.Key.Nome,
                TotalDespesas = x
                    .Where(x => x.Transacao != null && x.Transacao.TipoTransacao == TipoTransacao.Despesa)
                    .Sum(x => x.Transacao != null? x.Transacao.Valor : 0),
                TotalReceitas = x
                    .Where(x => x.Transacao != null && x.Transacao.TipoTransacao == TipoTransacao.Receita)
                    .Sum(x => x.Transacao != null? x.Transacao.Valor : 0),
            })
            .ToListAsync(cancellationToken);
    }

    public Task<ResumoFinanceiroValoresResponse> ObterResumoFinanceiroTotal(CancellationToken cancellationToken = default)
    {
        return _context.Transacaos
            .GroupBy(x => 1)
            .Select(x => new ResumoFinanceiroValoresResponse()
            {
                TotalDespesas = x
                    .Where(x => x.TipoTransacao == TipoTransacao.Despesa)
                    .Sum(x => x.Valor),
                TotalReceitas = x
                    .Where(x => x.TipoTransacao == TipoTransacao.Receita)
                    .Sum(x => x.Valor),
            })
            .FirstAsync(cancellationToken);
    }
}
