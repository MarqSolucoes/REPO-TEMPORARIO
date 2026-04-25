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
    public class StatusSolicitacaoCompraConfiguration : IEntityTypeConfiguration<StatusSolicitacaoCompra>
    {
        public void Configure(EntityTypeBuilder<StatusSolicitacaoCompra> builder)
        {
            builder.ToTable(nameof(StatusSolicitacaoCompra), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.Descricao)
                .IsRequired();
        }
    }
}
