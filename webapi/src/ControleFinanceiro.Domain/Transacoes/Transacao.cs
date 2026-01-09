using FluentResults;

namespace ControleFinanceiro.Domain.Transacoes;

public class Transacao : AggregateRoot
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

    internal static Result<Transacao> Criar(string descricao, decimal valor, TipoTransacao tipoTransacao, Guid categoriaId, DateTime? data)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Fail("Descricao é obrigatório/a e não pode ser vazio/a ou conter apenas espaços em branco");

        if (valor <= 0)
            return Result.Fail("O valor da transação deve ser maior que zero");

        var transacao = new Transacao(
            Guid.NewGuid(),
            descricao,
            valor,
            tipoTransacao,
            categoriaId,
            data ?? DateTime.UtcNow,
            DateTime.UtcNow);

        return transacao;
    }
}
