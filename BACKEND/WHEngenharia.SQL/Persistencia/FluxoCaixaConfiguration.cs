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
    public class FluxoCaixaConfiguration : IEntityTypeConfiguration<FluxoCaixa>
    {
        public void Configure(EntityTypeBuilder<FluxoCaixa> builder)
        {
            builder.ToTable(nameof(FluxoCaixa), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdTipoFluxoCaixa).IsRequired();

            builder.Property(x => x.IdFluxoCaixaPai);

            builder.Property(x => x.Cancelado).IsRequired();

            builder.Property(x => x.IdObra);

            builder.Property(x => x.CodigoObra);

            builder.Property(x => x.IdDef).IsRequired();

            builder.Property(x => x.CodigoDef).IsRequired();

            builder.Property(x => x.IdPedidoCompra);

            builder.Property(x => x.CodigoPedidoCompra);

            builder.Property(x => x.IdPedidoCompraFatura);
            
            builder.Property(x => x.CodigoFatura);

            builder.Property(x => x.IdPedidoCompraNotaFiscal);

            builder.Property(x => x.NumeroNotaFiscalPedidoCompra);

            builder.Property(x => x.IdPedidoInterno);

            builder.Property(x => x.CodigoPedidoInterno);

            builder.Property(x => x.IdCliente);

            builder.Property(x => x.NomeCliente);

            builder.Property(x => x.IdFaturamento);

            builder.Property(x => x.NumeroNotaFiscalFaturamento);

            builder.Property(x => x.IdFornecedorBeneficiario);

            builder.Property(x => x.IdUsuarioBeneficiario);

            builder.Property(x => x.DataLancamento).IsRequired();

            builder.Property(x => x.DataPagamento).IsRequired();

            builder.Property(x => x.Valor);

            builder.Property(x => x.PagamentoEfetuado).IsRequired();

            builder.Property(x => x.DataPagamentoEfetuado);

            builder.Property(x => x.IdUsuarioInformouPagamento);

            builder.HasOne(x => x.Fornecedor)
               .WithOne()
               .HasForeignKey<FluxoCaixa>(x => x.IdFornecedorBeneficiario);

            builder.HasOne(x => x.Obra)
               .WithOne()
               .HasForeignKey<FluxoCaixa>(x => x.IdObra);

            builder.HasOne(x => x.UsuarioBeneficiario)
               .WithOne()
               .HasForeignKey<FluxoCaixa>(x => x.IdUsuarioBeneficiario);

            builder.HasOne(x => x.UsuarioPagamento)
               .WithOne()
               .HasForeignKey<FluxoCaixa>(x => x.IdUsuarioInformouPagamento);

            builder.HasOne(x => x.FluxoCaixaPai)
               .WithMany(x => x.FluxosCaixaFilhos)
               .HasForeignKey(x => x.IdFluxoCaixaPai)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
