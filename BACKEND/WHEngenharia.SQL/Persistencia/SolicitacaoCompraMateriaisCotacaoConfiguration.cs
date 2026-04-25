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
    public class SolicitacaoCompraMateriaisCotacaoConfiguration : IEntityTypeConfiguration<SolicitacaoCompraMateriaisCotacao>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoCompraMateriaisCotacao> builder)
        {
            builder.ToTable("SolicitacaoCompra_MateriaisCotacao", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdSolicitacaoCompraMaterial)
                .IsRequired();

            builder.Property(x => x.IdFornecedor)
                .IsRequired();

            builder.Property(x => x.IdCondicaoPagamento)
                .IsRequired();

            builder.Property(x => x.QuantidadeCotado)
                .IsRequired();

            builder.Property(x => x.ValorUnitarioCotado)
                .IsRequired();

            builder.Property(x => x.ValorUnitarioComDesconto)
                .IsRequired();

            builder.Property(x => x.DataEntrega);

            builder.Property(x => x.CotacaoFinal)
                .IsRequired();

            builder.Property(x => x.CotacaoMaisBarata)
                .IsRequired();

            builder.Property(x => x.Frete);

            builder.Property(x => x.Imposto);
            builder.Property(x => x.Desconto);

            builder.HasOne(x => x.CondicaoPagamento)
               .WithOne()
               .HasForeignKey<SolicitacaoCompraMateriaisCotacao>(x => x.IdCondicaoPagamento)
               .IsRequired();

            builder.HasMany(x => x.PagamentoManual)
               .WithOne(x => x.SolicitacaoCompraMateriaisCotacao);
        }
    }
}
