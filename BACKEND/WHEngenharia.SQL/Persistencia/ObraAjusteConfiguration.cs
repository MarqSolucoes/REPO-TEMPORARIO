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
    public class ObraAjusteConfiguration : IEntityTypeConfiguration<ObraAjuste>
    {
        public void Configure(EntityTypeBuilder<ObraAjuste> builder)
        {
            builder.ToTable("Obra_Ajuste", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.NomeLogicoAntes)
                .IsRequired();

            builder.Property(x => x.NomeLogicoDepois)
                .IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro).IsRequired();

            builder.HasOne(x => x.Usuario)
                .WithOne()
                .HasForeignKey<ObraAjuste>(x => x.IdUsuarioCadastro)
                .IsRequired();

        }
    }
}
