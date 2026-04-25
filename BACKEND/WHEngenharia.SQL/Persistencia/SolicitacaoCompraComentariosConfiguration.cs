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
    public class SolicitacaoCompraComentariosConfiguration : IEntityTypeConfiguration<SolicitacaoCompraComentarios>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoCompraComentarios> builder)
        {
            builder.ToTable("SolicitacaoCompra_Comentarios", "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdSolicitacaoCompra)
                .IsRequired();

            builder.Property(x => x.Observacao)
                .IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();
        }
    }
}
