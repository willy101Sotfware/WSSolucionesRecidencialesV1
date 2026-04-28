namespace Application.DTOs;

public class CreateBuildingRequest
{
    public string Nombre { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? Ciudad { get; set; }
    public string? Departamento { get; set; }
    public string? Pais { get; set; }
    public string? Nit { get; set; }
    public int? CompanyId { get; set; }
}

public class UpdateBuildingRequest
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
}

public class BuildingResponse
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
    public string? CompanyName { get; set; }
}
