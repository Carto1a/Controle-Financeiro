using FluentResults;

namespace ControleFinanceiro.Domain.Transacoes;

public class Categoria
{
    public Guid Id { get; private set; }
    public string Descricao { get; private set; }
    public Finalidade Finalidade { get; private set; }

    private Categoria() { }
    private Categoria(Guid id, string descricao, Finalidade finalidade)
    {
        Id = id;
        Descricao = descricao;
        Finalidade = finalidade;
    }

    public Result TipoTransacaoValido(TipoTransacao tipo)
    {
        if (tipo == TipoTransacao.Despesa && Finalidade == Finalidade.Receita)
            return Result.Fail("A transação do tipo 'Despesa' é incompatível com a finalidade 'Receita'");

        if (tipo == TipoTransacao.Receita && Finalidade == Finalidade.Despesa)
            return Result.Fail("A transação do tipo 'Receita' é incompatível com a finalidade 'Despesa'");

        return Result.Ok();
    }
}
