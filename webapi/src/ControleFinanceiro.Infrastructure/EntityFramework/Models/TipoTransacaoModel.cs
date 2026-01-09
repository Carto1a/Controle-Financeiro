using ControleFinanceiro.Domain.Transacoes;

namespace ControleFinanceiro.Infrastructure.EntityFramework.Models;

public class TipoTransacaoModel
{
    public TipoTransacao Id { get; set; }
    public string Nome { get; set; } = null!;
}
