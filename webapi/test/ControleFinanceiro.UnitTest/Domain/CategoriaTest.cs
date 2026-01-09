using ControleFinanceiro.Domain.Transacoes;

namespace ControleFinanceiro.UnitTest.Domain;

public class CategoriaTests
{
    [Fact]
    public void Criar_DeveCriarCategoria_QuandoDadosValidos()
    {
        var result = Categoria.Criar(
            "Alimentação",
            "Gastos com comida",
            Finalidade.Despesa);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_DeveFalhar_QuandoNomeVazioOuEspacos(string nome)
    {
        var result = Categoria.Criar(
            nome,
            "Descrição válida",
            Finalidade.Despesa);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message.Contains("Nome é obrigatório"));
    }

    [Fact]
    public void Criar_DeveFalhar_QuandoNomeNulo()
    {
        string nome = null!;

        var result = Categoria.Criar(
            nome,
            "Descrição válida",
            Finalidade.Despesa);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message.Contains("Nome é obrigatório"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_DeveFalhar_QuandoDescricaoVaziaOuEspacos(string descricao)
    {
        var result = Categoria.Criar(
            "Transporte",
            descricao,
            Finalidade.Despesa);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message.Contains("Descricao é obrigatório"));
    }

    [Fact]
    public void Criar_DeveFalhar_QuandoDescricaoNula()
    {
        string descricao = null!;

        var result = Categoria.Criar(
            "Transporte",
            descricao,
            Finalidade.Despesa);

        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message.Contains("Descricao é obrigatório"));
    }

    [Fact]
    public void TipoTransacaoValido_DeveFalhar_QuandoDespesaComFinalidadeReceita()
    {
        var categoria = Categoria
            .Criar("Salário", "Recebimento mensal", Finalidade.Receita)
            .Value;

        var result = categoria.TipoTransacaoValido(TipoTransacao.Despesa);

        Assert.True(result.IsFailed);
    }

    [Fact]
    public void TipoTransacaoValido_DeveFalhar_QuandoReceitaComFinalidadeDespesa()
    {
        var categoria = Categoria
            .Criar("Mercado", "Compras do mês", Finalidade.Despesa)
            .Value;

        var result = categoria.TipoTransacaoValido(TipoTransacao.Receita);

        Assert.True(result.IsFailed);
    }

    [Fact]
    public void TipoTransacaoValido_DeveRetornarOk_QuandoTiposCompativeis()
    {
        var despesa = Categoria
            .Criar("Aluguel", "Moradia", Finalidade.Despesa)
            .Value;

        var receita = Categoria
            .Criar("Salário", "Pagamento mensal", Finalidade.Receita)
            .Value;

        Assert.True(despesa.TipoTransacaoValido(TipoTransacao.Despesa).IsSuccess);
        Assert.True(receita.TipoTransacaoValido(TipoTransacao.Receita).IsSuccess);
    }

    [Theory]
    [InlineData(TipoTransacao.Despesa)]
    [InlineData(TipoTransacao.Receita)]
    public void TipoTransacaoValido_DeveRetornarOk_QuandoFinalidadeAmbas(TipoTransacao tipo)
    {
        var categoria = Categoria
            .Criar("Transferência", "Ambas", Finalidade.Ambas)
            .Value;

        var result = categoria.TipoTransacaoValido(tipo);

        Assert.True(result.IsSuccess);
    }
}
