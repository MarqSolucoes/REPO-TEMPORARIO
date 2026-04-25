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
    public class StatusMedicaoConfiguration : IEntityTypeConfiguration<StatusMedicao>
    {
        public void Configure(EntityTypeBuilder<StatusMedicao> builder)
        {
            builder.ToTable(nameof(StatusMedicao), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.Descricao)
                .IsRequired();
        }
    }
}