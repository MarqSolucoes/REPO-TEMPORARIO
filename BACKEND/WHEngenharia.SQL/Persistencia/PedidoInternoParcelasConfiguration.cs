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
    public class PedidoInternoParcelasConfiguration : IEntityTypeConfiguration<PedidoInternoParcelas>
    {
        public void Configure(EntityTypeBuilder<PedidoInternoParcelas> builder)
        {
            builder.ToTable("PedidoInterno_Parcelas", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.IdPedidoInterno).IsRequired();

            builder.Property(x => x.Parcela).IsRequired();

            builder.Property(x => x.Valor).IsRequired();

            builder.Property(x => x.CodigoFormatado).IsRequired();

            builder.Property(x => x.DataPagamento).IsRequired();

            builder.Property(x => x.PagamentoEfetuado).IsRequired();

            builder.HasMany(x => x.ParcelaObras)
               .WithOne(x => x.PedidoInternoParcela);

            builder.HasMany(x => x.ParcelaDEFs)
              .WithOne(x => x.PedidoInternoParcela);
        }
    }
}
