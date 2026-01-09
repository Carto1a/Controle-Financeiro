using ControleFinanceiro.Domain.Pessoas;
using ControleFinanceiro.Domain.Transacoes;

namespace ControleFinanceiro.UnitTest.Domain;

public class TransacaoTests
{
    [Fact]
    public void Cadastrar_DeveFalhar_QuandoPessoaMenorDeIdadeEReceita()
    {
        // Arrange
        var pessoa = Pessoa
            .Criar("João", DateTime.Today.AddYears(-10))
            .Value;

        var categoria = Categoria
            .Criar("Salário", "Recebimento", Finalidade.Receita)
            .Value;

        // Act
        var result = CadastrarTransacaoService.Cadastrar(
            pessoa,
            categoria,
            "Salário mensal",
            100m,
            TipoTransacao.Receita,
            DateTime.Today);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors,
            e => e.Message.Contains("menores de idade"));
    }

    [Fact]
    public void Cadastrar_DevePermitir_QuandoPessoaMenorDeIdadeEDespesa()
    {
        // Arrange
        var pessoa = Pessoa
            .Criar("João", DateTime.Today.AddYears(-15))
            .Value;

        var categoria = Categoria
            .Criar("Lanche", "Alimentação", Finalidade.Despesa)
            .Value;

        // Act
        var result = CadastrarTransacaoService.Cadastrar(
            pessoa,
            categoria,
            "Lanche da escola",
            15m,
            TipoTransacao.Despesa,
            DateTime.Today);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(TipoTransacao.Despesa, result.Value.TipoTransacao);
    }

    [Fact]
    public void Cadastrar_DeveFalhar_QuandoCategoriaNaoAceitaTipoTransacao()
    {
        // Arrange
        var pessoa = Pessoa
            .Criar("Carlos", DateTime.Today.AddYears(-30))
            .Value;

        var categoria = Categoria
            .Criar("Salário", "Receita mensal", Finalidade.Receita)
            .Value;

        // Act
        var result = CadastrarTransacaoService.Cadastrar(
            pessoa,
            categoria,
            "Compra no mercado",
            200m,
            TipoTransacao.Despesa,
            DateTime.Today);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors,
            e => e.Message.Contains("incompatível"));
    }

    [Fact]
    public void Cadastrar_DeveCriarTransacao_QuandoTodasRegrasForemValidas()
    {
        // Arrange
        var pessoa = Pessoa
            .Criar("Carlos", DateTime.Today.AddYears(-25))
            .Value;

        var categoria = Categoria
            .Criar("Salário", "Pagamento mensal", Finalidade.Receita)
            .Value;

        var data = new DateTime(2025, 1, 1);

        // Act
        var result = CadastrarTransacaoService.Cadastrar(
            pessoa,
            categoria,
            "Salário Janeiro",
            3000m,
            TipoTransacao.Receita,
            data);

        // Assert
        Assert.True(result.IsSuccess);

        var transacao = result.Value;
        Assert.Equal("Salário Janeiro", transacao.Descricao);
        Assert.Equal(3000m, transacao.Valor);
        Assert.Equal(TipoTransacao.Receita, transacao.TipoTransacao);
        Assert.Equal(categoria.Id, transacao.CategoriaId);
        Assert.Equal(data, transacao.Data);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_DeveFalhar_QuandoDescricaoVaziaOuEspacos(string descricao)
    {
        // Arrange
        var pessoa = Pessoa
            .Criar("Carlos", DateTime.Today.AddYears(-30))
            .Value;

        var categoria = Categoria
            .Criar("Mercado", "Compras", Finalidade.Despesa)
            .Value;

        // Act
        var result = CadastrarTransacaoService.Cadastrar(
            pessoa,
            categoria,
            descricao,
            100m,
            TipoTransacao.Despesa,
            DateTime.Today);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message.Contains("Descricao é obrigatório"));
    }

    [Fact]
    public void Cadastrar_DevePropagarErro_QuandoTransacaoInvalida()
    {
        // Arrange
        var pessoa = Pessoa
            .Criar("Carlos", DateTime.Today.AddYears(-30))
            .Value;

        var categoria = Categoria
            .Criar("Mercado", "Compras", Finalidade.Despesa)
            .Value;

        string descricao = null!;

        // Act
        var result = CadastrarTransacaoService.Cadastrar(
            pessoa,
            categoria,
            descricao,
            100m,
            TipoTransacao.Despesa,
            DateTime.Today);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors,
            e => e.Message.Contains("Descricao"));
    }

    [Fact]
    public void Criar_DeveUsarDataAtual_QuandoDataForNull()
    {
        // Arrange
        var antes = DateTime.Now;

        var pessoa = Pessoa
            .Criar("Carlos", DateTime.Today.AddYears(-25))
            .Value;

        var categoria = Categoria
            .Criar("Salário", "Pagamento mensal", Finalidade.Receita)
            .Value;

        // Act
        var result = CadastrarTransacaoService.Cadastrar(
            pessoa,
            categoria,
            "Salário Janeiro",
            3000m,
            TipoTransacao.Receita,
            null);

        var depois = DateTime.Now;

        // Assert
        Assert.True(result.IsSuccess);

        var data = result.Value.Data;
        Assert.True(data >= antes && data <= depois);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Criar_DeveFalhar_QuandoValorMenorOuIgualZero(decimal valor)
    {
        // Arrange
        var pessoa = Pessoa
            .Criar("Carlos", DateTime.Today.AddYears(-25))
            .Value;

        var categoria = Categoria
            .Criar("Salário", "Pagamento mensal", Finalidade.Receita)
            .Value;

        var data = new DateTime(2025, 1, 1);

        // Act
        var result = CadastrarTransacaoService.Cadastrar(
            pessoa,
            categoria,
            "Salário Janeiro",
            valor,
            TipoTransacao.Receita,
            data);

        // Assert
        Assert.True(result.IsFailed);
    }
}
