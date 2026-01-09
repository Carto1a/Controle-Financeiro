using FluentResults;

namespace ControleFinanceiro.Domain.Transacoes;

public class Categoria
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Descricao { get; private set; }
    public Finalidade Finalidade { get; private set; }

    private Categoria() { }
    private Categoria(Guid id, string nome, string descricao, Finalidade finalidade)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
        Finalidade = finalidade;
    }

    public static Result<Categoria> Criar(string nome, string descricao, Finalidade finalidade)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Fail("Descricao é obrigatório/a e não pode ser vazio/a ou conter apenas espaços em branco");

        if (string.IsNullOrWhiteSpace(nome))
            return Result.Fail("Nome é obrigatório/a e não pode ser vazio/a ou conter apenas espaços em branco");

        var categoria = new Categoria(Guid.NewGuid(), nome, descricao, finalidade);

        return categoria;
    }

    public Result TipoTransacaoValido(TipoTransacao tipo)
    {
        if (tipo == TipoTransacao.Despesa && Finalidade == Finalidade.Receita)
            return Result.Fail("Transação do tipo 'Despesa' é incompatível com a finalidade 'Receita'");

        if (tipo == TipoTransacao.Receita && Finalidade == Finalidade.Despesa)
            return Result.Fail("Transação do tipo 'Receita' é incompatível com a finalidade 'Despesa'");

        return Result.Ok();
    }
}
