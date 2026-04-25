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
    public class FaturamentoRecebimentoConfiguration : IEntityTypeConfiguration<FaturamentoRecebimento>
    {
        public void Configure(EntityTypeBuilder<FaturamentoRecebimento> builder)
        {
            builder.ToTable("Faturamento_Recebimento", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdFaturamento).IsRequired();

            builder.Property(x => x.DataRecebimento).IsRequired();

            builder.Property(x => x.ValorRecebido).IsRequired();
        }
    }
}
