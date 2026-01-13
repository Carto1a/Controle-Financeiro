using ControleFinanceiro.Domain.Pessoas;
using FluentResults;

namespace ControleFinanceiro.Domain.Transacoes;

/// <summary>
/// Serviço de domínio para cadastrar uma transação.
///
/// Usado como Service porque a operação envolve **três agregados**:
/// - Pessoa: para validar idade e associar a transação
/// - Categoria: para validar se o tipo de transação é permitido
/// - Transacao: para criar o registro
///
/// Como essa regra cruza múltiplos agregados, não pertence a nenhum único deles,
/// então é modelada como um serviço de domínio.
/// </summary>
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
