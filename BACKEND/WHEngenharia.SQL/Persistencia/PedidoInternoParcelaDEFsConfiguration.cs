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
    public class PedidoInternoParcelaDEFsConfiguration : IEntityTypeConfiguration<PedidoInternoParcelaDEFs>
    {
        public void Configure(EntityTypeBuilder<PedidoInternoParcelaDEFs> builder)
        {
            builder.ToTable("PedidoInterno_ParcelaDEFs", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.IdPedidoInternoParcela).IsRequired();

            builder.Property(x => x.IdDef).IsRequired();

            builder.Property(x => x.Valor).IsRequired();

            builder.HasOne(x => x.DEF)
               .WithOne()
               .HasForeignKey<PedidoInternoParcelaDEFs>(x => x.IdDef)
               .IsRequired();
        }
    }
}
