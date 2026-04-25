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
    public class AgendaConfiguration : IEntityTypeConfiguration<Agenda>
    {
        public void Configure(EntityTypeBuilder<Agenda> builder)
        {
            builder.ToTable(nameof(Agenda), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdObra);

            builder.Property(x => x.CodigoObra);

            builder.Property(x => x.IdDef).IsRequired();

            builder.Property(x => x.CodigoDef).IsRequired();

            builder.Property(x => x.IdPedidoCompra);

            builder.Property(x => x.CodigoPedidoCompra);

            builder.Property(x => x.IdPedidoInterno);

            builder.Property(x => x.CodigoPedidoInterno);

            builder.Property(x => x.IdFornecedorBeneficiario);

            builder.Property(x => x.NomeFantasiaFornecedor);

            builder.Property(x => x.IdUsuarioBeneficiario);

            builder.Property(x => x.NomeUsuarioBeneficiario);

            builder.Property(x => x.IdCliente);

            builder.Property(x => x.NomeCliente);

            builder.Property(x => x.NumeroNF);

            builder.Property(x => x.DataLancamento).IsRequired();

            builder.Property(x => x.DataPagamento).IsRequired();

            builder.Property(x => x.Valor);

            builder.Property(x => x.PagamentoEfetuado).IsRequired();
        }
    }
}
