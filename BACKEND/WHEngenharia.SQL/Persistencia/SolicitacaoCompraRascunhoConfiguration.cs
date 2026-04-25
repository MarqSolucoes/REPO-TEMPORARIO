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
    public class SolicitacaoCompraRascunhoConfiguration : IEntityTypeConfiguration<SolicitacaoCompraRascunho>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoCompraRascunho> builder)
        {
            builder.ToTable("SolicitacaoCompra_Rascunho", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.Titulo).IsRequired();
            builder.Property(x => x.ObjetoSerializado).IsRequired();
            builder.Property(x => x.IdUsuarioCadastro).IsRequired();
            builder.Property(x => x.DataCadastro).IsRequired();
        }
    }
}
