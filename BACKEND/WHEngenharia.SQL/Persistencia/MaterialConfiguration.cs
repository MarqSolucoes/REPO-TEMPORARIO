using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Persistencia
{
    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.ToTable(nameof(Material), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.IdCategoriaMaterial)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .IsRequired();

            builder.Property(x => x.Ativo)
                .IsRequired();

            builder.Property(x => x.Servico)
                .IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasOne(x => x.CategoriaMaterial)
                .WithOne()
                .HasForeignKey<Material>(x => x.IdCategoriaMaterial)
                .IsRequired();

            builder.HasOne(x => x.UnidadeMaterial)
                .WithOne()
                .HasForeignKey<Material>(x => x.IdUnidadeMaterial)
                .IsRequired();
        }
    }
}
