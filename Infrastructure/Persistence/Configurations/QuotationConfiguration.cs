using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class QuotationConfiguration : IEntityTypeConfiguration<Quotation>
{
    public void Configure(EntityTypeBuilder<Quotation> builder)
    {
        builder.ToTable("Quotations");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(q => q.Numero)
            .IsUnique();

        builder.Property(q => q.Numero)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(q => q.Fecha)
            .HasMaxLength(50);

        builder.Property(q => q.Asunto)
            .HasMaxLength(255);

        builder.Property(q => q.FirmaNombre)
            .HasMaxLength(255);

        builder.Property(q => q.FirmaCargo)
            .HasMaxLength(255);

        builder.Property(q => q.FirmaCelular)
            .HasMaxLength(20);

        builder.Property(q => q.PlazoEntrega)
            .HasMaxLength(100);

        builder.Property(q => q.Garantia)
            .HasMaxLength(100);

        builder.Property(q => q.ShowPlazo)
            .HasDefaultValue(0);

        builder.Property(q => q.ShowGarantia)
            .HasDefaultValue(0);

        builder.HasOne(q => q.Building)
            .WithMany(b => b.Quotations)
            .HasForeignKey(q => q.IdEdificio)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(q => q.QuotationItems)
            .WithOne(qi => qi.Quotation)
            .HasForeignKey(qi => qi.IdCotizacion)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
