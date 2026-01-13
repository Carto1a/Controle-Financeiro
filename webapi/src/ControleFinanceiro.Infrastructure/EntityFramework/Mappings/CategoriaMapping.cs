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

        // Mapeia a propriedade Finalidade como shadow property "FinalidadeId".
        // Aqui foi usado shadow property apenas para simplificar o mapeamento, sem criar
        // um model separado, já que Finalidade é um valor derivado do domínio.
        // Em projetos maiores, poderia ser model completo, mas para este caso funcionou bem assim.
        builder.Property<Finalidade>("FinalidadeId");

        builder.HasOne<FinalidadeModel>()
            .WithMany()
            .HasForeignKey("FinalidadeId")
            .OnDelete(DeleteBehavior.ClientSetNull);

        builder.Ignore(x => x.Finalidade);
    }
}
