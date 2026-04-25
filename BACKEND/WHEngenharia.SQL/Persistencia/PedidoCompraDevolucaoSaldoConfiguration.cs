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
    public class PedidoCompraDevolucaoSaldoConfiguration : IEntityTypeConfiguration<PedidoCompraDevolucaoSaldo>
    {
        public void Configure(EntityTypeBuilder<PedidoCompraDevolucaoSaldo> builder)
        {
            builder.ToTable("PedidoCompra_DevolucaoSaldo", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdPedidoCompra)
                .IsRequired();

            builder.Property(x => x.IdMotivoDevolucaoSaldo)
                .IsRequired();

            builder.Property(x => x.Valor)
                .IsRequired();

            builder.Property(x => x.Observacao)
                .IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasOne(x => x.MotivoDevolucao)
              .WithOne()
              .HasForeignKey<PedidoCompraDevolucaoSaldo>(x => x.IdMotivoDevolucaoSaldo)
              .IsRequired();
        }
    }
}
