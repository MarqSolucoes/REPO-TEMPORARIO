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
    public class FaturamentoArquivosConfiguration : IEntityTypeConfiguration<FaturamentoArquivos>
    {
        public void Configure(EntityTypeBuilder<FaturamentoArquivos> builder)
        {
            builder.ToTable("Faturamento_Arquivos", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdFaturamento)
                .IsRequired();

            builder.Property(x => x.Nome)
                .IsRequired();

            builder.Property(x => x.NomeLogico)
                .IsRequired();

            builder.Property(x => x.Extensao)
                .IsRequired();

            builder.Property(x => x.TamanhoMB)
                .IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();
        }
    }
}
