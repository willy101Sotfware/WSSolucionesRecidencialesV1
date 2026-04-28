using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BuildingConfiguration : IEntityTypeConfiguration<Building>
{
    public void Configure(EntityTypeBuilder<Building> builder)
    {
        builder.ToTable("Buildings");

        builder.HasKey(b => b.IdEdificio);

        builder.Property(b => b.IdEdificio)
            .ValueGeneratedOnAdd();

        builder.Property(b => b.Nombre)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.Email)
            .HasMaxLength(100);

        builder.Property(b => b.Telefono)
            .HasMaxLength(20);

        builder.Property(b => b.Direccion)
            .HasMaxLength(255);

        builder.Property(b => b.Ciudad)
            .HasMaxLength(100);

        builder.Property(b => b.Departamento)
            .HasMaxLength(100);

        builder.Property(b => b.Pais)
            .HasMaxLength(100);

        builder.Property(b => b.Nit)
            .HasMaxLength(20);

        builder.Property(b => b.Activo)
            .HasDefaultValue(1);

        builder.HasOne(b => b.Company)
            .WithMany(c => c.Buildings)
            .HasForeignKey(b => b.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(b => b.Employees)
            .WithOne(e => e.Building)
            .HasForeignKey(e => e.BuildingId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(b => b.Quotations)
            .WithOne(q => q.Building)
            .HasForeignKey(q => q.IdEdificio)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
