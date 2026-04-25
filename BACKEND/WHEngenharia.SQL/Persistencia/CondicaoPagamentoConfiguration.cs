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
    public class CondicaoPagamentoConfiguration : IEntityTypeConfiguration<CondicaoPagamento>
    {
        public void Configure(EntityTypeBuilder<CondicaoPagamento> builder)
        {
            builder.ToTable(nameof(CondicaoPagamento), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.Descricao)
                .IsRequired();

            builder.Property(x => x.Ativo)
                .IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasMany(x => x.Parcelas)
                .WithOne(x => x.CondicaoPagamento);
        }
    }
}
