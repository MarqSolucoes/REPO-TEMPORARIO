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
    public class PedidoInternoParcelaObrasConfiguration : IEntityTypeConfiguration<PedidoInternoParcelaObras>
    {
        public void Configure(EntityTypeBuilder<PedidoInternoParcelaObras> builder)
        {
            builder.ToTable("PedidoInterno_ParcelaObras", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.IdPedidoInternoParcela).IsRequired();

            builder.Property(x => x.IdObra).IsRequired();

            builder.Property(x => x.Valor).IsRequired();
        }
    }
}
