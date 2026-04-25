using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Persistencia
{
    public class PedidoInternoRecorrenteConfiguration : IEntityTypeConfiguration<PedidoInternoRecorrente>
    {
        public void Configure(EntityTypeBuilder<PedidoInternoRecorrente> builder)
        {
            builder.ToTable(nameof(PedidoInternoRecorrente), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.IdDEF)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .IsRequired();

            builder.Property(x => x.DiaGeracao)
                .IsRequired();

            builder.Property(x => x.DataLimiteGeracao);

            builder.Property(x => x.Valor)
                .IsRequired();

            builder.Property(x => x.NecessitaConfirmacao)
                .IsRequired();

            builder.Property(x => x.Ativo)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAprovador);

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasOne(x => x.DEF)
                .WithOne()
                .HasForeignKey<PedidoInternoRecorrente>(x => x.IdDEF)
                .IsRequired();

            builder.HasOne(x => x.UsuarioCadastro)
                .WithOne()
                .HasForeignKey<PedidoInternoRecorrente>(x => x.IdUsuarioCadastro)
                .IsRequired();
        }
    }

}
