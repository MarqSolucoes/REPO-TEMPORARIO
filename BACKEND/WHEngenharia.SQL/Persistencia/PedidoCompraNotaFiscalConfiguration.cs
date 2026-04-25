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
    public class PedidoCompraNotaFiscalConfiguration : IEntityTypeConfiguration<PedidoCompraNotaFiscal>
    {
        public void Configure(EntityTypeBuilder<PedidoCompraNotaFiscal> builder)
        {
            builder.ToTable("PedidoCompra_NotaFiscal", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdPedidoCompra)
                .IsRequired();

            builder.Property(x => x.IdPedidoCompraArquivo)
                .IsRequired();

            builder.Property(x => x.IdFornecedor);

            builder.Property(x => x.Nome)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .IsRequired();

            builder.Property(x => x.NumeroNotaFiscal)
                .IsRequired();

            builder.Property(x => x.Aprovada);

            builder.Property(x => x.DevolucaoVisualizada).IsRequired();

            builder.Property(x => x.PagamentoEfetuado)
                .IsRequired();

            builder.Property(x => x.Valor)
                .IsRequired();

            builder.Property(x => x.ValorImposto)
                .IsRequired();

            builder.Property(x => x.ValorFrete)
                .IsRequired();

            builder.Property(x => x.DataAprovacao);

            builder.Property(x => x.DataVencimento)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAprovacao);

            builder.Property(x => x.ImportadoParaFinanceiro);

            builder.Property(x => x.Cancelada).IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasOne(x => x.Arquivo)
             .WithOne()
             .HasForeignKey<PedidoCompraNotaFiscal>(x => x.IdPedidoCompraArquivo)
             .IsRequired();

            builder.HasOne(x => x.Fornecedor)
             .WithOne()
             .HasForeignKey<PedidoCompraNotaFiscal>(x => x.IdFornecedor);

            builder.HasOne(x => x.UsuarioDiretorAprovador)
            .WithOne()
            .HasForeignKey<PedidoCompraNotaFiscal>(x => x.IdUsuarioAprovacao);

            builder.HasMany(x => x.Materiais)
               .WithOne(x => x.NotaFiscal);

            builder.HasMany(x => x.Pagamentos)
              .WithOne(x => x.PedidoCompraNotaFiscal);

        }
    }
}
