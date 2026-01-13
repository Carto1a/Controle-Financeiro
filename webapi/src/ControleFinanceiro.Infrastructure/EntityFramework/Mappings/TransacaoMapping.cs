using ControleFinanceiro.Domain.Transacoes;
using ControleFinanceiro.Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.EntityFramework.Mappings;

public class TransacaoMapping : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        builder.ToTable("Transacoes");

        // Mapeia TipoTransacao como shadow property "TipoTransacaoId"
        // Evita criar uma propriedade de navegação explícita, simplificando o projeto
        builder.Property<TipoTransacao>("TipoTransacaoId");

        builder.HasOne<TipoTransacaoModel>()
            .WithMany()
            .HasForeignKey("TipoTransacaoId")
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Ignore(x => x.TipoTransacao);
    }
}
