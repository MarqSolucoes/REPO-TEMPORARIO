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
    public class HistoricoConfiguration : IEntityTypeConfiguration<Historico>
    {
        public void Configure(EntityTypeBuilder<Historico> builder)
        {
            builder.ToTable(nameof(Historico), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdSolicitacaoCompra);

            builder.Property(x => x.IdPedidoCompra);

            builder.Property(x => x.IdPedidoInterno);

            builder.Property(x => x.Campo)
                .IsRequired();

            builder.Property(x => x.ValorAntigo);

            builder.Property(x => x.ValorNovo);

            builder.Property(x => x.Data)
                .IsRequired();

            builder.Property(x => x.IdUsuario)
                .IsRequired();

            builder.Property(x => x.Usuario)
                .IsRequired();
        }
    }
}
