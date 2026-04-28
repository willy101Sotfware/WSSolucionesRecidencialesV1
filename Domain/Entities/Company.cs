namespace Domain.Entities;

public class Company
{
    public int IdEmpresa { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Nit { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public int? Activo { get; set; }

    public ICollection<Building> Buildings { get; set; } = new List<Building>();
}
