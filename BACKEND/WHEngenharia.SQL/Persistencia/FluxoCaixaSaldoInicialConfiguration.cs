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
    public class FluxoCaixaSaldoInicialConfiguration : IEntityTypeConfiguration<FluxoCaixaSaldoInicial>
    {
        public void Configure(EntityTypeBuilder<FluxoCaixaSaldoInicial> builder)
        {
            builder.ToTable(nameof(FluxoCaixaSaldoInicial), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.Mes).IsRequired();

            builder.Property(x => x.Ano).IsRequired();

            builder.Property(x => x.Saldo).IsRequired();
            builder.Property(x => x.SaldoCalculado).IsRequired();
        }
    }
}
