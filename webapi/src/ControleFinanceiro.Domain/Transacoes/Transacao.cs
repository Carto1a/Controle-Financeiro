using ControleFinanceiro.Domain.Pessoas;
using FluentResults;

namespace ControleFinanceiro.Domain.Transacoes;

public class Transacao
{
    public Guid Id { get; private set; }
    public string Descricao { get; private set; }
    public decimal Valor { get; private set; }
    public TipoTransacao TipoTransacao { get; private set; }
    public Guid CategoriaId { get; private set; }
    public DateTime Data { get; private set; }
    public DateTime CriadoEm { get; private set; }

    private Transacao() { }
    private Transacao(Guid id, string descricao, decimal valor, TipoTransacao tipoTransacao, Guid categoriaId, DateTime data, DateTime criadoEm)
    {
        Id = id;
        Descricao = descricao;
        Valor = valor;
        TipoTransacao = tipoTransacao;
        CategoriaId = categoriaId;
        Data = data;
        CriadoEm = criadoEm;
    }

    public static Result<Transacao> Criar(string descricao, decimal valor, Pessoa pessoa, TipoTransacao tipoTransacao, Categoria categoria, DateTime? data)
    {
        if (pessoa.MenorDeIdade() && tipoTransacao != TipoTransacao.Despesa)
            return Result.Fail("Pessoas menores de idade só podem registrar transações do tipo 'Despesa'");

        var resultCategoria = categoria.TipoTransacaoValido(tipoTransacao);
        if (resultCategoria.IsFailed)
            return resultCategoria;

        var transacao = new Transacao(
            Guid.NewGuid(),
            descricao,
            valor,
            tipoTransacao,
            categoria.Id,
            data ?? DateTime.Now,
            DateTime.Now);

        return transacao;
    }
}
