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
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable(nameof(Cliente), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdCidade)
                .IsRequired();

            builder.Property(x => x.RazaoSocial)    
                .IsRequired();

            builder.Property(x => x.NomeFantasia)
                .IsRequired();

            builder.Property(x => x.CNPJ)
                .IsRequired();

            builder.Property(x => x.InscricaoEstadual);

            builder.Property(x => x.EmailFinanceiro);

            builder.Property(x => x.EmailComercial);

            builder.Property(x => x.ResponsavelComercial);

            builder.Property(x => x.TelefoneCelular);
            
            builder.Property(x => x.TelefoneFixo);
            
            builder.Property(x => x.CEP);
            
            builder.Property(x => x.Endereco);

            builder.Property(x => x.Bairro);

            builder.Property(x => x.Observacao);

            builder.Property(x => x.DiasDePagamento).IsRequired();

            builder.Property(x => x.Ativo)
                .IsRequired();

            builder.Property(x => x.IdUsuarioCadastro)
                .IsRequired();

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.IdUsuarioAlteracao)
                .IsRequired();

            builder.Property(x => x.DataUltimaAlteracao)
                .IsRequired();

            builder.HasOne(x => x.Cidade)
                .WithOne()
                .HasForeignKey<Cliente>(x => x.IdCidade)
                .IsRequired();
        }
    }
}
