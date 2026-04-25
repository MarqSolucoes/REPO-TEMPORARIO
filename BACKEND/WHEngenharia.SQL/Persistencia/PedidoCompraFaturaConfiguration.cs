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
    class PedidoCompraFaturaConfiguration : IEntityTypeConfiguration<PedidoCompraFatura>
    {
        public void Configure(EntityTypeBuilder<PedidoCompraFatura> builder)
        {
            builder.ToTable("PedidoCompra_Faturas", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdPedidoCompra)
                .IsRequired();

            builder.Property(x => x.CodigoFormatado)
                .IsRequired();

            builder.Property(x => x.DataFatura)
                .IsRequired();

            builder.Property(x => x.Valor)
                .IsRequired();

            builder.Property(x => x.PagamentoEfetuado)
                .IsRequired();
        }
    }
}
