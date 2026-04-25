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
    public class SolicitacaoCompraConfiguration : IEntityTypeConfiguration<SolicitacaoCompra>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoCompra> builder)
        {
            builder.ToTable(nameof(SolicitacaoCompra), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdStatusSolicitacaoCompra)
                .IsRequired();

            builder.Property(x => x.IdTipoCentroCusto)
                .IsRequired();

            builder.Property(x => x.IdCentroCustoObra);

            builder.Property(x => x.IdCentroCustoDEF);

            builder.Property(x => x.Nome);

            builder.Property(x => x.Codigo)
                .IsRequired();

            builder.Property(x => x.CodigoSequencia)
                .IsRequired();

            builder.Property(x => x.CodigoAno)
                .IsRequired();

            builder.Property(x => x.Observacao);

            builder.Property(x => x.ObservacaoDeAprovacao);

            builder.Property(x => x.ObservacaoParaFornecedor);

            builder.Property(x => x.MotivoCancelamento);

            builder.Property(x => x.ValorEstimado);

            builder.Property(x => x.ValorTotalCotado);

            builder.Property(x => x.ValorMelhorCotacao);

            builder.Property(x => x.DataEntrega);
            
            builder.Property(x => x.Servico).IsRequired();

            builder.Property(x => x.IdEngenheiroAprovador);

            builder.Property(x => x.DataAprovacaoEngenheiro);

            builder.Property(x => x.IdUsuarioFinalizacaoCotacao);

            builder.Property(x => x.DataFinalizacaoCotacao);

            builder.Property(x => x.IdDiretorAprovador);

            builder.Property(x => x.DataAprovacaoDiretor);

            builder.Property(x => x.IdUsuarioComprador);

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasOne(x => x.UsuarioCadastro)
               .WithOne()
               .HasForeignKey<SolicitacaoCompra>(x => x.IdUsuarioCadastro)
               .IsRequired();

            builder.HasOne(x => x.UsuarioComprador)
               .WithOne()
               .HasForeignKey<SolicitacaoCompra>(x => x.IdUsuarioComprador);

            builder.HasOne(x => x.StatusSolicitacaoCompra)
                .WithOne()
                .HasForeignKey<SolicitacaoCompra>(x => x.IdStatusSolicitacaoCompra)
                .IsRequired();

            builder.HasOne(x => x.TipoCentroCusto)
                .WithOne()
                .HasForeignKey<SolicitacaoCompra>(x => x.IdTipoCentroCusto)
                .IsRequired();

            builder.HasOne(x => x.CentroCustoDEF)
              .WithOne()
              .HasForeignKey<SolicitacaoCompra>(x => x.IdCentroCustoDEF);

            builder.HasOne(x => x.CentroCustoObra)
              .WithOne()
              .HasForeignKey<SolicitacaoCompra>(x => x.IdCentroCustoObra);

            builder.HasMany(x => x.Materiais)
                .WithOne(x => x.SolicitacaoCompra);

            builder.HasMany(x => x.Arquivos)
              .WithOne(x => x.SolicitacaoCompra);

            builder.HasMany(x => x.Comentarios)
              .WithOne(x => x.SolicitacaoCompra);
        }
    }
}
