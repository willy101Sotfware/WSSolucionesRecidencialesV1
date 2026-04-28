namespace Application.DTOs;

public class CreateCompanyRequest
{
    public string Nombre { get; set; } = string.Empty;
    public string? Nit { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
}

public class UpdateCompanyRequest
{
    public int IdEmpresa { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Nit { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public int? Activo { get; set; }
}

public class CompanyResponse
{
    public int IdEmpresa { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Nit { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public int? Activo { get; set; }
}
