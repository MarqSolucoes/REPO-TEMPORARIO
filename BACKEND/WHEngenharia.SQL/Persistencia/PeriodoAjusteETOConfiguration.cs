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
    public class PeriodoAjusteETOConfiguration : IEntityTypeConfiguration<PeriodoAjusteETO>
    {
        public void Configure(EntityTypeBuilder<PeriodoAjusteETO> builder)
        {
            builder.ToTable(nameof(PeriodoAjusteETO), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.DataInicial)
                .IsRequired();

            builder.Property(x => x.DataFinal)
                .IsRequired();
        }
    }
}
