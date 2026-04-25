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
    public class ObraUsuarioAprovacaoConfiguration : IEntityTypeConfiguration<ObraUsuarioAprovacao>
    {
        public void Configure(EntityTypeBuilder<ObraUsuarioAprovacao> builder)
        {
            builder.ToTable("Obra_UsuarioAprovacao", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdObra)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAprovacao)
                .IsRequired();


            builder.HasOne(x => x.Usuario)
                .WithOne()
                .HasForeignKey<ObraUsuarioAprovacao>(x => x.IdUsuarioAprovacao)
                .IsRequired();
        }
    }
}
