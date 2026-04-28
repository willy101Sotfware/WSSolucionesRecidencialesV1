namespace Domain.Entities;

public class Employee
{
    public int IdEmpleado { get; set; }
    public string? NumeroDocumento { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public string? Barrio { get; set; }
    public string? FechaIngreso { get; set; }
    public int? Activo { get; set; }
    public int? BuildingId { get; set; }

    public Building? Building { get; set; }
}
