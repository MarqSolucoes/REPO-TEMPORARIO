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
    public class FaturamentoConfiguration : IEntityTypeConfiguration<Faturamento>
    {
        public void Configure(EntityTypeBuilder<Faturamento> builder)
        {
            builder.ToTable(nameof(Faturamento), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdObra)
                .IsRequired();

            builder.Property(x => x.IdStatusFaturamento)
                .IsRequired();

            builder.Property(x => x.NumeroNF)
                .IsRequired();

            builder.Property(x => x.DataFaturamento)
                .IsRequired();

            builder.Property(x => x.DataRecebimentoPrevisto)
                .IsRequired();

            builder.Property(x => x.DataRecebimentoRealizado);

            builder.Property(x => x.Observacao)
                .IsRequired();

            builder.Property(x => x.ValorBruto)
                .IsRequired();

            builder.Property(x => x.ValorINSS)
                .IsRequired();

            builder.Property(x => x.ValorISS)
                .IsRequired();

            builder.Property(x => x.ValorIR)
                .IsRequired();

            builder.Property(x => x.ValorArt30)
                .IsRequired();

            builder.Property(x => x.ValorDesconto)
                .IsRequired();

            builder.Property(x => x.ValorMaterial)
                .IsRequired();

            builder.Property(x => x.ValorSinal)
                .IsRequired();

            builder.Property(x => x.ValorLiquido)
                .IsRequired();

            builder.Property(x => x.ValorRecebido)
                .IsRequired();

            builder.Property(x => x.ValorNaoComissionado)
                .IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasOne(x => x.Status)
                .WithOne()
                .HasForeignKey<Faturamento>(x => x.IdStatusFaturamento)
                .IsRequired();

            builder.HasOne(x => x.Obra)
              .WithOne()
              .HasForeignKey<Faturamento>(x => x.IdObra);

            builder.HasOne(x => x.UsuarioCadastro)
              .WithOne()
              .HasForeignKey<Faturamento>(x => x.IdUsuarioCadastro);

            builder.HasMany(x => x.Arquivos)
              .WithOne(x => x.Faturamento);
        }
    }
}
