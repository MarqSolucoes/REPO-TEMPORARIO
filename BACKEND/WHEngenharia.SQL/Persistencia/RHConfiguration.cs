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
    public class RHConfiguration : IEntityTypeConfiguration<RH>
    {
        public void Configure(EntityTypeBuilder<RH> builder)
        {
            builder.ToTable(nameof(RH), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.IdFuncionario)
                .IsRequired();

            builder.Property(x => x.MesReferencia)
                .IsRequired();

            builder.Property(x => x.AnoReferencia)
                .IsRequired();

            builder.Property(x => x.Salario).IsRequired();
            builder.Property(x => x.SalarioHoraExtra).IsRequired();
            builder.Property(x => x.SalarioDesconto).IsRequired();
            builder.Property(x => x.SalarioComissao).IsRequired();
            builder.Property(x => x.ValeRefeicaoWH).IsRequired();
            builder.Property(x => x.ValeRefeicaoFuncionario).IsRequired();
            builder.Property(x => x.ValeTransporteWH).IsRequired();
            builder.Property(x => x.AssistenciaMedicaWH).IsRequired();
            builder.Property(x => x.AssistenciaMedicaFuncionario).IsRequired();
            builder.Property(x => x.AssistenciaOdontologicaWH).IsRequired();
            builder.Property(x => x.AssistenciaOdontologicaFuncionario).IsRequired();
            builder.Property(x => x.SeguroWH).IsRequired();
            builder.Property(x => x.SeguroFuncionario).IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasOne(x => x.Funcionario)
                .WithOne()
                .HasForeignKey<RH>(x => x.IdFuncionario)
                .IsRequired();
        }
    }
}
