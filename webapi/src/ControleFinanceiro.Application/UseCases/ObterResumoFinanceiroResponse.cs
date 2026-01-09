namespace ControleFinanceiro.Application.UseCases;

public record ResumoFinanceiroValoresResponse
{
    public decimal TotalReceitas { get; init; }
    public decimal TotalDespesas { get; init; }
    public decimal Saldo => TotalReceitas - TotalDespesas;
}

public record ResumoFinanceiroResponse : ResumoFinanceiroValoresResponse
{
    public Guid PessoaId { get; init; }
    public string Nome { get; init; } = default!;
}
