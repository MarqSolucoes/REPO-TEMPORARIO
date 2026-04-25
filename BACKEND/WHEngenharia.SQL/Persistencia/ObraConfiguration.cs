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
    public class ObraConfiguration : IEntityTypeConfiguration<Obra>
    {
        public void Configure(EntityTypeBuilder<Obra> builder)
        {
            builder.ToTable(nameof(Obra), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdCliente)
                .IsRequired();

            builder.Property(x => x.IdCidade)
                .IsRequired();

            builder.Property(x => x.IdUsuarioDiretorAprovador)
                .IsRequired();

            builder.Property(x => x.Codigo)
                .IsRequired();

            builder.Property(x => x.CodigoSequencia)
                .IsRequired();

            builder.Property(x => x.CodigoAno)
                .IsRequired();

            builder.Property(x => x.CodigoProposta)
                .IsRequired();

            builder.Property(x => x.NumeroPedidoCliente);

            builder.Property(x => x.CEP)
                .IsRequired();

            builder.Property(x => x.EnderecoObra)
                .IsRequired();

            builder.Property(x => x.EnderecoEntrega)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .IsRequired();

            builder.Property(x => x.DataProposta)
                .IsRequired();

            builder.Property(x => x.DataInicio)
                .IsRequired();

            builder.Property(x => x.DataFim)
                .IsRequired();

            builder.Property(x => x.PrazoDias)
                .IsRequired();

            builder.Property(x => x.DiasDePagamento)
                .IsRequired();

            builder.Property(x => x.ValorTotal)
                .IsRequired();

            builder.Property(x => x.ValorMaterial)
                .IsRequired();  

            builder.Property(x => x.ValorNaoComissionado)
                .IsRequired();

            builder.Property(x => x.AliquotaImpostoISS)
                .IsRequired();

            builder.Property(x => x.AliquotaImpostoINSS);

            builder.Property(x => x.AliquotaImpostoIR);

            builder.Property(x => x.AliquotaImpostoArt30);

            builder.Property(x => x.ValorSinal);

            builder.Property(x => x.PercentualEquivalenteSinal);

            builder.Property(x => x.ETO).IsRequired();

            builder.Property(x => x.Gasto).IsRequired();

            builder.Property(x => x.Saldo).IsRequired();

            builder.Property(x => x.DataRecebimentoSinal);

            builder.Property(x => x.Bloqueada)
                .IsRequired();

            builder.Property(x => x.Cancelada)
                .IsRequired();

            builder.Property(x => x.AprovacaoAutomatica)
                .IsRequired();

            builder.Property(x => x.Finalizada)
                .IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasOne(x => x.Cliente)
                .WithOne()
                .HasForeignKey<Obra>(x => x.IdCliente)
                .IsRequired();

            builder.HasOne(x => x.Cidade)
                .WithOne()
                .HasForeignKey<Obra>(x => x.IdCidade)
                .IsRequired();

            builder.HasOne(x => x.DiretorAprovador)
                .WithOne()
                .HasForeignKey<Obra>(x => x.IdUsuarioDiretorAprovador)
                .IsRequired();

            builder.HasMany(x => x.Faturamentos)
                .WithOne(x => x.Obra);

            builder.HasMany(x => x.ETOs)
                .WithOne(x => x.Obra);

            builder.HasMany(x => x.UsuariosAprovadores)
               .WithOne(x => x.Obra);
        }
    }
}
