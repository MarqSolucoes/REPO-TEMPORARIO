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
    public class PedidoInternoConfiguration : IEntityTypeConfiguration<PedidoInterno>
    {
        public void Configure(EntityTypeBuilder<PedidoInterno> builder)
        {
            builder.ToTable(nameof(PedidoInterno), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).IsRequired();

            builder.Property(x => x.IdPedidoInternoRecorrente);

            builder.Property(x => x.IdUsuarioAprovacao)
                .IsRequired();

            builder.Property(x => x.IdFornecedorBeneficiario);

            builder.Property(x => x.IdUsuarioBeneficiario);

            builder.Property(x => x.IdDef).IsRequired();

            builder.Property(x => x.IdObra);

            builder.Property(x => x.Codigo)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .IsRequired();

            builder.Property(x => x.NumeroTotalParcelas)
                .IsRequired();

            builder.Property(x => x.ValorTotal)
                .IsRequired();

            builder.Property(x => x.Aprovado);

            builder.Property(x => x.DataAprovacao);

            builder.Property(x => x.ImportadoParaFinanceiro);

            builder.Property(x => x.Cancelado).IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasOne(x => x.UsuarioAprovacao)
                .WithOne()
                .HasForeignKey<PedidoInterno>(x => x.IdUsuarioAprovacao)
                .IsRequired();

            builder.HasOne(x => x.FornecedorBeneficiario)
                .WithOne()
                .HasForeignKey<PedidoInterno>(x => x.IdFornecedorBeneficiario);

            builder.HasOne(x => x.UsuarioBeneficiario)
                .WithOne()
                .HasForeignKey<PedidoInterno>(x => x.IdUsuarioBeneficiario);

            builder.HasOne(x => x.UsuarioCadastro)
                .WithOne()
                .HasForeignKey<PedidoInterno>(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.HasOne(x => x.DEF)
                .WithOne()
                .HasForeignKey<PedidoInterno>(x => x.IdDef)
                .IsRequired();

            builder.HasOne(x => x.Obra)
                .WithOne()
                .HasForeignKey<PedidoInterno>(x => x.IdObra);

            builder.HasMany(x => x.Arquivos)
             .WithOne(x => x.PedidoInterno);

            builder.HasMany(x => x.Parcelas)
               .WithOne(x => x.PedidoInterno);

            builder.HasMany(x => x.Obras)
               .WithOne(x => x.PedidoInterno);
        }
    }
}
