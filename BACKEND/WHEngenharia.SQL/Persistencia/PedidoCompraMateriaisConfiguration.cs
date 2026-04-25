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
    public class PedidoCompraMateriaisConfiguration : IEntityTypeConfiguration<PedidoCompraMateriais>
    {
        public void Configure(EntityTypeBuilder<PedidoCompraMateriais> builder)
        {
            builder.ToTable("PedidoCompra_Materiais", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdPedidoCompra)
                .IsRequired();

            builder.Property(x => x.IdMaterial)
                .IsRequired();

            builder.Property(x => x.Quantidade)
                .IsRequired();

            builder.Property(x => x.QuantidadeConciliada)
                .IsRequired();

            builder.Property(x => x.ValorUnitario)
                .IsRequired();

            builder.Property(x => x.ValorTotal)
                .IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();
        }
    }
}
