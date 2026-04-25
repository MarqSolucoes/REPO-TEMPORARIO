using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Persistencia
{
    public class TipoCentroCustoConfiguration : IEntityTypeConfiguration<TipoCentroCusto>
    {
        public void Configure(EntityTypeBuilder<TipoCentroCusto> builder)
        {
            builder.ToTable(nameof(TipoCentroCusto), "dbo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);

            builder.Property(x => x.Descricao)
                .IsRequired();
        }
    }
}
