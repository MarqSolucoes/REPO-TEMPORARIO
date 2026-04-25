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
    public class PedidoCompraNotaFiscalMateriaisConfiguration : IEntityTypeConfiguration<PedidoCompraNotaFiscalMateriais>
    {
        public void Configure(EntityTypeBuilder<PedidoCompraNotaFiscalMateriais> builder)
        {
            builder.ToTable("PedidoCompra_NotaFiscal_Materiais", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdPedidoCompraMateriais)
                .IsRequired();

            builder.Property(x => x.IdPedidoCompraNotaFiscal)
                .IsRequired();

            builder.Property(x => x.Quantidade)
                .IsRequired();

            builder.Property(x => x.Valor)
                .IsRequired();
        }
    }
}
