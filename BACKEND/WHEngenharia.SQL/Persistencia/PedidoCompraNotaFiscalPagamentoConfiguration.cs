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
    public class PedidoCompraNotaFiscalPagamentoConfiguration : IEntityTypeConfiguration<PedidoCompraNotaFiscalPagamento>
    {
        public void Configure(EntityTypeBuilder<PedidoCompraNotaFiscalPagamento> builder)
        {
            builder.ToTable("PedidoCompra_NotaFiscal_Pagamento", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdPedidoCompraNotaFiscal)
                .IsRequired();

            builder.Property(x => x.ValorBruto)
                .IsRequired(); ;

            builder.Property(x => x.ValorMaterialAbatido)
                .IsRequired(); ;

            builder.Property(x => x.ValorBaseCalculo)
                .IsRequired(); ;

            builder.Property(x => x.AliquotaIR);

            builder.Property(x => x.AliquotaArt30);

            builder.Property(x => x.AliquotaINSS);

            builder.Property(x => x.AliquotaISS);

            builder.Property(x => x.ValorIR);

            builder.Property(x => x.ValorArt30);

            builder.Property(x => x.ValorINSS);

            builder.Property(x => x.ValorISS);

            builder.Property(x => x.IdTipoCalculoNotaFiscalIR);

            builder.Property(x => x.IdTipoCalculoNotaFiscalArt30);

            builder.Property(x => x.IdTipoCalculoNotaFiscalINSS);

            builder.Property(x => x.IdTipoCalculoNotaFiscalISS);

            builder.Property(x => x.DataPagamentoIR);

            builder.Property(x => x.DataPagamentoArt30);

            builder.Property(x => x.DataPagamentoINSS);

            builder.Property(x => x.DataPagamentoISS);

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
