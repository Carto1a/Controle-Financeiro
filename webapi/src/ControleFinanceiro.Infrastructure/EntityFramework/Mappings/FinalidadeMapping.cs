using ControleFinanceiro.Domain.Transacoes;
using ControleFinanceiro.Infrastructure.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFinanceiro.Infrastructure.EntityFramework.Mappings;

public class FinalidadeMapping : IEntityTypeConfiguration<FinalidadeModel>
{
    public void Configure(EntityTypeBuilder<FinalidadeModel> builder)
    {
        builder.ToTable("Finalidades");

        var finalidade = Enum.GetValues<Finalidade>()
            .Select(x => new FinalidadeModel()
            {
                Id = x,
                Nome = x.ToString()
            });

        builder.HasData(finalidade);
    }
}

