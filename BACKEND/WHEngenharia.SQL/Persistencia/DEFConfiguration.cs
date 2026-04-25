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
    public class DEFConfiguration : IEntityTypeConfiguration<DEF>
    {
        public void Configure(EntityTypeBuilder<DEF> builder)
        {
            builder.ToTable(nameof(DEF), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdTipoPedidoInterno)
                .IsRequired();

            builder.Property(x => x.Codigo)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .IsRequired();

            builder.Property(x => x.Ativo)
                .IsRequired();

            builder.Property(x => x.Tipo)
                .IsRequired();

            builder.Property(x => x.PodeAbrirPedidoInternoRecorrente)
                .IsRequired();

            builder.Property(x => x.FinanceiroPodeSerAlterado)
                .IsRequired();

            builder.Property(x => x.FinanceiroPodeTerValorSubdividido)
                .IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();
        }
    }
}
