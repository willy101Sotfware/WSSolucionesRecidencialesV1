namespace Application.DTOs;

public class CreateQuotationItemRequest
{
    public int IdCotizacion { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal? Cantidad { get; set; }
    public string? UnidadMedida { get; set; }
    public string? Imagen { get; set; }
    public decimal? ValorUnitario { get; set; }
    public decimal? ValorTotal { get; set; }
    public string? PlazoEntrega { get; set; }
    public int? ShowPlazo { get; set; }
    public string? Garantia { get; set; }
    public int? ShowGarantia { get; set; }
}

public class UpdateQuotationItemRequest
{
    public int Id { get; set; }
    public int IdCotizacion { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal? Cantidad { get; set; }
    public string? UnidadMedida { get; set; }
    public string? Imagen { get; set; }
    public decimal? ValorUnitario { get; set; }
    public decimal? ValorTotal { get; set; }
    public string? PlazoEntrega { get; set; }
    public int? ShowPlazo { get; set; }
    public string? Garantia { get; set; }
    public int? ShowGarantia { get; set; }
}

public class QuotationItemResponse
{
    public int Id { get; set; }
    public int IdCotizacion { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal? Cantidad { get; set; }
    public string? UnidadMedida { get; set; }
    public string? Imagen { get; set; }
    public decimal? ValorUnitario { get; set; }
    public decimal? ValorTotal { get; set; }
    public string? PlazoEntrega { get; set; }
    public int? ShowPlazo { get; set; }
    public string? Garantia { get; set; }
    public int? ShowGarantia { get; set; }
}
