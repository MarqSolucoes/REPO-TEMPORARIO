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
    public class ObraFaturamentoConfiguration : IEntityTypeConfiguration<ObraFaturamento>
    {
        public void Configure(EntityTypeBuilder<ObraFaturamento> builder)
        {
            builder.ToTable("Obra_Faturamento", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdObra)
                .IsRequired();

            builder.Property(x => x.Data)
                .IsRequired();

            builder.Property(x => x.DataPrevistaRecebimento).IsRequired();

            builder.Property(x => x.Valor).IsRequired();

            builder.Property(x => x.ValorFaturado).IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasOne(x => x.UsuarioCadastro)
                .WithOne()
                .HasForeignKey<ObraFaturamento>(x => x.IdUsuarioCadastro);
        }
    }
}
