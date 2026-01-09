using ControleFinanceiro.Domain.Transacoes;
using ControleFinanceiro.Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.EntityFramework.Mappings;

public class CategoriaMapping : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

        builder.Property<Finalidade>("FinalidadeId");

        builder.HasOne<FinalidadeModel>()
            .WithMany()
            .HasForeignKey("FinalidadeId")
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Ignore(x => x.Finalidade);
    }
}
