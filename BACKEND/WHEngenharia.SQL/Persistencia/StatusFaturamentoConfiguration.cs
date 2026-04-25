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
    public class StatusFaturamentoConfiguration : IEntityTypeConfiguration<StatusFaturamento>
    {
        public void Configure(EntityTypeBuilder<StatusFaturamento> builder)
        {
            builder.ToTable(nameof(StatusFaturamento), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.Descricao)
                .IsRequired();
        }
    }
}
