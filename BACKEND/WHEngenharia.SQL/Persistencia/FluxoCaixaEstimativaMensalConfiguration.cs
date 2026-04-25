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
    public class FluxoCaixaEstimativaMensalConfiguration : IEntityTypeConfiguration<FluxoCaixaEstimativaMensal>
    {
        public void Configure(EntityTypeBuilder<FluxoCaixaEstimativaMensal> builder)
        {
            builder.ToTable(nameof(FluxoCaixaEstimativaMensal), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.Mes).IsRequired();

            builder.Property(x => x.Ano).IsRequired();

            builder.Property(x => x.OrdemCompraPedidoInterno).IsRequired();
            
            builder.Property(x => x.NotaFiscal).IsRequired();
            
            builder.Property(x => x.ETO).IsRequired();
            
            builder.Property(x => x.FolhaPagamento).IsRequired();
            
            builder.Property(x => x.Imposto).IsRequired();
            
            builder.Property(x => x.DespesasFixas).IsRequired();
            
            builder.Property(x => x.Reserva).IsRequired();
            
            builder.Property(x => x.Outros).IsRequired();
            
            builder.Property(x => x.Transferencias).IsRequired();
            
            builder.Property(x => x.TotalDiario).IsRequired();
            
            builder.Property(x => x.AReceberFaturado).IsRequired();
            
            builder.Property(x => x.AReceberAFaturar).IsRequired();
            
            builder.Property(x => x.Estornos).IsRequired();
            
            builder.Property(x => x.Saldo).IsRequired();
        }
    }
}
