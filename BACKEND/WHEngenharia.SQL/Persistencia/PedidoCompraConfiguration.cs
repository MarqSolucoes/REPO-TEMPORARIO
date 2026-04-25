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
    public class PedidoCompraConfiguration : IEntityTypeConfiguration<PedidoCompra>
    {
        public void Configure(EntityTypeBuilder<PedidoCompra> builder)
        {
            builder.ToTable(nameof(PedidoCompra), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdSolicitacaoCompra)
                .IsRequired();

            builder.Property(x => x.IdStatusPedidoCompra)
                .IsRequired();

            builder.Property(x => x.IdFornecedor)
                .IsRequired();

            builder.Property(x => x.IdTipoCentroCusto)
                .IsRequired();

            builder.Property(x => x.IdCentroCustoObra);

            builder.Property(x => x.IdCentroCustoDEF);

            builder.Property(x => x.IdCondicaoPagamento)
                .IsRequired();

            builder.Property(x => x.CodigoSequencia)
                .IsRequired();

            builder.Property(x => x.CodigoAno)
                .IsRequired();

            builder.Property(x => x.Codigo)
                .IsRequired();

            builder.Property(x => x.ValorTotal)
                .IsRequired();

            builder.Property(x => x.ValorDesconto)
                .IsRequired();

            builder.Property(x => x.MotivoCancelamento);
            
            builder.Property(x => x.DataEntrega)
                .IsRequired();

            builder.Property(x => x.ImportadoParaFinanceiro);

            builder.Property(x => x.EnderecoEntrega);

            builder.Property(x => x.Frete).IsRequired();

            builder.Property(x => x.Imposto).IsRequired();

            builder.Property(x => x.Saldo).IsRequired();

            builder.Property(x => x.ObservacaoParaFornecedor).IsRequired();


            builder.Property(x => x.IdUsuarioComprador);

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasMany(x => x.Faturas)
               .WithOne(x => x.PedidoCompra);

            builder.HasMany(x => x.Materiais)
               .WithOne(x => x.PedidoCompra);

            builder.HasMany(x => x.Arquivos)
              .WithOne(x => x.PedidoCompra);

            builder.HasMany(x => x.NotasFiscais)
              .WithOne(x => x.PedidoCompra);

            builder.HasMany(x => x.CancelamentosSaldo)
              .WithOne(x => x.PedidoCompra);

            builder.HasOne(x => x.UsuarioComprador)
               .WithOne()
               .HasForeignKey<PedidoCompra>(x => x.IdUsuarioComprador);
        }
    }
}
