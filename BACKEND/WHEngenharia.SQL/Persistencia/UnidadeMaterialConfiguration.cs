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
    public class UnidadeMaterialConfiguration : IEntityTypeConfiguration<UnidadeMaterial>
    {
        public void Configure(EntityTypeBuilder<UnidadeMaterial> builder)
        {
            builder.ToTable(nameof(UnidadeMaterial), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Codigo)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .IsRequired();
        }
    }
}
