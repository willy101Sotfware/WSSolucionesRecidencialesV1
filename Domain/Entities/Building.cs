namespace Domain.Entities;

public class Building
{
    public int IdEdificio { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? Ciudad { get; set; }
    public string? Departamento { get; set; }
    public string? Pais { get; set; }
    public string? Nit { get; set; }
    public int? Activo { get; set; }
    public int? CompanyId { get; set; }

    public Company? Company { get; set; }
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
}
