using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Persistencia
{
    public class SolicitacaoCompraMateriaisConfiguration : IEntityTypeConfiguration<SolicitacaoCompraMateriais>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoCompraMateriais> builder)
        {
            builder.ToTable("SolicitacaoCompra_Materiais", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdSolicitacaoCompra)
                .IsRequired();

            builder.Property(x => x.IdMaterial)
                .IsRequired();

            builder.Property(x => x.Quantidade)
                .IsRequired();

            builder.Property(x => x.ValorUnitarioEstimado);

            builder.Property(x => x.IdTipoCentroCusto)
                .IsRequired();

            builder.Property(x => x.IdCentroCustoObra);

            builder.Property(x => x.IdCentroCustoDEF);

            builder.HasMany(x => x.Cotacoes)
                .WithOne(x => x.Material);
        }
    }
}
