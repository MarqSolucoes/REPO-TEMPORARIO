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
    public class StatusPedidoCompraConfiguration : IEntityTypeConfiguration<StatusPedidoCompra>
    {
        public void Configure(EntityTypeBuilder<StatusPedidoCompra> builder)
        {
            builder.ToTable(nameof(StatusPedidoCompra), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.Descricao)
                .IsRequired();
        }
    }
}
