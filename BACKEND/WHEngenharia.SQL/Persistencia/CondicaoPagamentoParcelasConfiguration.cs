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
    public class CondicaoPagamentoParcelasConfiguration : IEntityTypeConfiguration<CondicaoPagamento_Parcelas>
    {
        public void Configure(EntityTypeBuilder<CondicaoPagamento_Parcelas> builder)
        {
            builder.ToTable(nameof(CondicaoPagamento_Parcelas), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdCondicaoPagamento)
                .IsRequired();

            builder.Property(x => x.DiasCorridos)
                .IsRequired();

            builder.Property(x => x.PorcentagemValorTotal)
                .IsRequired();
        }
    }
}
