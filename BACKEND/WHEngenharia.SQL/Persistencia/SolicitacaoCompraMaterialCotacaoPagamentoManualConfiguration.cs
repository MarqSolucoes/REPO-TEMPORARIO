using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Persistencia
{
    internal class SolicitacaoCompraMaterialCotacaoPagamentoManualConfiguration : IEntityTypeConfiguration<SolicitacaoCompraMaterialCotacaoPagamentoManual>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoCompraMaterialCotacaoPagamentoManual> builder)
        {
            builder.ToTable("SolicitacaoCompra_MateriaisCotacao_PagamentoManual", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdSolicitacaoCompraMaterialCotacao).IsRequired();

            builder.Property(x => x.Data).IsRequired();

            builder.Property(x => x.Valor).IsRequired();

            builder.Property(x => x.IdUsuarioCadastro).IsRequired();

            builder.Property(x => x.DataCadastro).IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao).IsRequired();

            builder.Property(x => x.DataUltimaAlteracao).IsRequired();
        }
    }
}
