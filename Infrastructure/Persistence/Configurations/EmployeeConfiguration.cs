using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.IdEmpleado);

        builder.Property(e => e.IdEmpleado)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.NumeroDocumento)
            .HasMaxLength(20);

        builder.Property(e => e.NombreCompleto)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Telefono)
            .HasMaxLength(20);

        builder.Property(e => e.Email)
            .HasMaxLength(100);

        builder.Property(e => e.Direccion)
            .HasMaxLength(255);

        builder.Property(e => e.Barrio)
            .HasMaxLength(100);

        builder.Property(e => e.FechaIngreso)
            .HasMaxLength(50);

        builder.Property(e => e.Activo)
            .HasDefaultValue(1);

        builder.HasOne(e => e.Building)
            .WithMany(b => b.Employees)
            .HasForeignKey(e => e.BuildingId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
