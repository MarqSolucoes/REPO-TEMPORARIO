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
    public class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.ToTable(nameof(Fornecedor), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.IdTipoFornecedor)
                .IsRequired();

            builder.Property(x => x.IdCidade)
                .IsRequired();

            builder.Property(x => x.IdCondicaoPagamento)
                .IsRequired();

            builder.Property(x => x.RazaoSocial);

            builder.Property(x => x.NomeFantasia);

            builder.Property(x => x.CNPJ);

            builder.Property(x => x.InscricaoEstadual);

            builder.Property(x => x.NumeroCadastral);

            builder.Property(x => x.Nome);

            builder.Property(x => x.CPF);

            builder.Property(x => x.Email);
            
            builder.Property(x => x.NomeVendedor);
            
            builder.Property(x => x.TelefoneCelular);
            
            builder.Property(x => x.TelefoneFixo);
            
            builder.Property(x => x.CEP);
            
            builder.Property(x => x.Endereco);
            
            builder.Property(x => x.Observacao);

            builder.Property(x => x.Bairro);

            builder.Property(x => x.Banco);

            builder.Property(x => x.Agencia);

            builder.Property(x => x.Conta);

            builder.Property(x => x.TipoConta);

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

            builder.HasOne(x => x.TipoFornecedor)
                .WithOne()
                .HasForeignKey<Fornecedor>(x => x.IdTipoFornecedor)
                .IsRequired();

            builder.HasOne(x => x.Cidade)
                .WithOne()
                .HasForeignKey<Fornecedor>(x => x.IdCidade)
                .IsRequired();

            builder.HasOne(x => x.CondicaoPagamento)
               .WithOne()
               .HasForeignKey<Fornecedor>(x => x.IdCondicaoPagamento)
               .IsRequired();
        }
    }
}
