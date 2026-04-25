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
    public class PedidoInternoObrasConfiguration : IEntityTypeConfiguration<PedidoInternoObras>
    {
        public void Configure(EntityTypeBuilder<PedidoInternoObras> builder)
        {
            builder.ToTable("PedidoInterno_Obras", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.IdPedidoInterno).IsRequired();

            builder.Property(x => x.IdObra).IsRequired();

            builder.Property(x => x.Valor).IsRequired();

            builder.HasOne(x => x.Obra)
                .WithOne()
                .HasForeignKey<PedidoInternoObras>(x => x.IdObra)
                .IsRequired();
        }
    }
}
