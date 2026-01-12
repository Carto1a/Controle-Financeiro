using ControleFinanceiro.Application.Abstractions.Data;

namespace ControleFinanceiro.Application.UseCases;

public record ObterResumoFinanceiroPaginatedQuery(int Pagina, int TamanhoPagina) : IPagination;

public record ResumoFinanceiroValoresResponse
{
    public decimal TotalReceitas { get; init; }
    public decimal TotalDespesas { get; init; }
    public decimal Saldo => TotalReceitas - TotalDespesas;
}

public record ResumoFinanceiroResponse : ResumoFinanceiroValoresResponse
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = default!;
}
