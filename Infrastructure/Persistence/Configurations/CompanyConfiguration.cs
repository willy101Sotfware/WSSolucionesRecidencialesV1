using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(c => c.IdEmpresa);

        builder.Property(c => c.IdEmpresa)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nombre)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(c => c.Nit)
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .HasMaxLength(100);

        builder.Property(c => c.Telefono)
            .HasMaxLength(20);

        builder.Property(c => c.Direccion)
            .HasMaxLength(255);

        builder.Property(c => c.Activo)
            .HasDefaultValue(1);
    }
}
