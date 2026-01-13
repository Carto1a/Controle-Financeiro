using ControleFinanceiro.Domain.Transacoes;
using ControleFinanceiro.Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.EntityFramework.Mappings;

/// <summary>
/// Configuração do mapeamento da entidade TipoTransacaoModel para EF Core.
/// Cria a tabela "TipoTransacoes" e popula com os valores do enum TipoTransacao.
/// </summary>
public class TipoTransacaoMapping : IEntityTypeConfiguration<TipoTransacaoModel>
{
    public void Configure(EntityTypeBuilder<TipoTransacaoModel> builder)
    {
        builder.ToTable("TipoTransacoes");

        var tipoTransacao = Enum.GetValues<TipoTransacao>()
            .Select(x => new TipoTransacaoModel()
            {
                Id = x,
                Nome = x.ToString()
            });

        builder.HasData(tipoTransacao);
    }
}
