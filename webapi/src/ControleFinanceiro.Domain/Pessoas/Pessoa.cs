using ControleFinanceiro.Domain.Transacoes;
using FluentResults;

namespace ControleFinanceiro.Domain.Pessoas;

public class Pessoa : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public List<Transacao> Transacoes { get; private set; } = [];
    public int Idade { get => CalcularIdade(DateTime.Today, DataNascimento); }

    private Pessoa() { }
    private Pessoa(Guid id, string nome, DateTime dataNascimento, List<Transacao> transacaos)
    {
        Id = id;
        Nome = nome;
        Transacoes = transacaos;
        DataNascimento = dataNascimento;
    }

    // Factory method para criar uma pessoa, já validando dados
    public static Result<Pessoa> Criar(string nome, DateTime dataNascimento)
    {
        var validacoes = Validar(nome, dataNascimento);
        if (validacoes.IsFailed) return validacoes;

        var pessoa = new Pessoa(Guid.NewGuid(), nome, dataNascimento, []);
        return pessoa;
    }

    public Result Atualizar(string nome, DateTime dataNascimento)
    {
        var validacoes = Validar(nome, dataNascimento);
        if (validacoes.IsFailed) return validacoes;

        Nome = nome;
        DataNascimento = dataNascimento;

        return Result.Ok();
    }

    // Marca a pessoa como deletada, dispara evento de domínio antes de salvar no banco
    public void MarcarDeletada()
    {
        AddDomainEvent(new PessoaDeletadaEvent(Id));
    }

    public void AdicionarTransacao(Transacao transacao) => Transacoes.Add(transacao);

    public static Result Validar(string nome, DateTime dataNascimento)
    {
        var erros = new List<Error>();

        if (string.IsNullOrWhiteSpace(nome))
            erros.Add(new("Nome é obrigatório/a e não pode ser vazio/a ou conter apenas espaços em branco"));
        if (CalcularIdade(DateTime.Today, dataNascimento) < 0)
            erros.Add(new("Data de nascimento é inválida, pois está no futuro"));

        return erros.Count > 0 ? Result.Fail(erros) : Result.Ok();
    }

    public bool MenorDeIdade() => Idade < 18;

    private static int CalcularIdade(DateTime dataReferencia, DateTime dataNascimento)
    {
        var idade = dataReferencia.Year - dataNascimento.Year;
        if (dataNascimento.Date.AddYears(idade) > dataReferencia)
            idade--;

        return idade;
    }
}
