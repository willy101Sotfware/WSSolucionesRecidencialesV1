using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class QuotationItemConfiguration : IEntityTypeConfiguration<QuotationItem>
{
    public void Configure(EntityTypeBuilder<QuotationItem> builder)
    {
        builder.ToTable("QuotationItems");

        builder.HasKey(qi => qi.Id);

        builder.Property(qi => qi.Id)
            .ValueGeneratedOnAdd();

        builder.Property(qi => qi.Descripcion)
            .IsRequired();

        builder.Property(qi => qi.UnidadMedida)
            .HasMaxLength(50);

        builder.Property(qi => qi.PlazoEntrega)
            .HasMaxLength(100);

        builder.Property(qi => qi.Garantia)
            .HasMaxLength(100);

        builder.Property(qi => qi.ShowPlazo)
            .HasDefaultValue(0);

        builder.Property(qi => qi.ShowGarantia)
            .HasDefaultValue(0);

        // FK removida temporalmente
        //builder.HasOne(qi => qi.Quotation)
        //    .WithMany(q => q.QuotationItems)
        //    .HasForeignKey(qi => qi.IdCotizacion)
        //    .OnDelete(DeleteBehavior.NoAction);
    }
}
