using ControleFinanceiro.Domain.Transacoes;

namespace ControleFinanceiro.Infrastructure.EntityFramework.Models;

public class FinalidadeModel
{
    public Finalidade Id { get; set; }
    public string Nome { get; set; } = null!;
}
