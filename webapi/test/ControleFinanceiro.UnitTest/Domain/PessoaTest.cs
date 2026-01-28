using ControleFinanceiro.Domain.Pessoas;

namespace ControleFinanceiro.UnitTest.Domain;

public class PessoaTests
{
    [Fact]
    public void Criar_DeveCriarPessoa_QuandoDadosValidos()
    {
        // Arrange
        var nome = "Carlos";
        var dataNascimento = DateTime.Today.AddYears(-30);

        // Act
        var result = Pessoa.Criar(nome, dataNascimento);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(nome, result.Value.Nome);
        Assert.Equal(dataNascimento, result.Value.DataNascimento);
        Assert.Empty(result.Value.Transacoes);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_DeveFalhar_QuandoNomeInvalido(string nome)
    {
        // Arrange
        var dataNascimento = DateTime.Today.AddYears(-20);

        // Act
        var result = Pessoa.Criar(nome, dataNascimento);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message.Contains("Nome é obrigatório"));
    }

    [Fact]
    public void Criar_DeveFalhar_QuandoNomeNulo()
    {
        // Arrange
        string nome = null!;

        // Act
        var result = Pessoa.Criar(nome, DateTime.Today.AddYears(-20));

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public void Criar_DeveFalhar_QuandoDataNascimentoNoFuturo()
    {
        // Arrange
        var nome = "Carlos";
        var dataNascimento = DateTime.Today.AddDays(1);

        // Act
        var result = Pessoa.Criar(nome, dataNascimento);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, e => e.Message.Contains("Data de nascimento"));
    }

    [Fact]
    public void Atualizar_DeveAtualizarDados_QuandoValidos()
    {
        // Arrange
        var pessoa = Pessoa.Criar("Carlos", DateTime.Today.AddYears(-25)).Value;
        var novoNome = "Eduardo";
        var novaData = DateTime.Today.AddYears(-30);

        // Act
        var result = pessoa.Atualizar(novoNome, novaData);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(novoNome, pessoa.Nome);
        Assert.Equal(novaData, pessoa.DataNascimento);
    }

    [Fact]
    public void Atualizar_DeveFalhar_QuandoDadosInvalidos()
    {
        // Arrange
        var pessoa = Pessoa.Criar("Carlos", DateTime.Today.AddYears(-25)).Value;

        // Act
        var result = pessoa.Atualizar("", DateTime.Today.AddDays(1));

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public void MenorDeIdade_DeveRetornarTrue_QuandoMenorQue18()
    {
        // Arrange
        var pessoa = Pessoa.Criar("João", DateTime.Today.AddYears(-10)).Value;

        // Act
        var menor = pessoa.MenorDeIdade();

        // Assert
        Assert.True(menor);
    }

    [Fact]
    public void MenorDeIdade_DeveRetornarFalse_QuandoMaiorOuIgual18()
    {
        // Arrange
        var pessoa = Pessoa.Criar("Maria", DateTime.Today.AddYears(-18)).Value;

        // Act
        var menor = pessoa.MenorDeIdade();

        // Assert
        Assert.False(menor);
    }

    [Fact]
    public void MarcarDeletada_DeveAdicionarEventoDeDominio()
    {
        // Arrange
        var pessoa = Pessoa.Criar("Carlos", DateTime.Today.AddYears(-30)).Value;

        // Act
        pessoa.MarcarDeletada();

        // Assert
        Assert.Single(pessoa.DomainEvents);
        Assert.IsType<PessoaDeletadaEvent>(pessoa.DomainEvents.First());
    }
}

