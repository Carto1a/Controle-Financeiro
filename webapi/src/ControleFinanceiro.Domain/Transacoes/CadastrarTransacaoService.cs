using ControleFinanceiro.Domain.Pessoas;
using FluentResults;

namespace ControleFinanceiro.Domain.Transacoes;

public class CadastrarTransacaoService
{
    public static Result<Transacao> Cadastrar(Pessoa pessoa, Categoria categoria, string descricao, decimal valor, TipoTransacao tipoTransacao, DateTime? data)
    {
        if (pessoa.MenorDeIdade() && tipoTransacao != TipoTransacao.Despesa)
            return Result.Fail("Pessoas menores de idade só podem registrar transações do tipo 'Despesa'");

        var resultCategoria = categoria.TipoTransacaoValido(tipoTransacao);
        if (resultCategoria.IsFailed)
            return resultCategoria;

        var result = Transacao.Criar(descricao, valor, tipoTransacao, categoria.Id, data);

        pessoa.AdicionarTransacao(result.Value);

        return result;
    }
}
