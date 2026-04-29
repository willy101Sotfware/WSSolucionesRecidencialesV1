namespace Application.DTOs;

public class CreateQuotationRequest
{
    public string Numero { get; set; } = string.Empty;
    public string? Fecha { get; set; }
    public int? IdEdificio { get; set; }
    public string? Asunto { get; set; }
    public string? CordialSaludo { get; set; }
    public string? HeaderPropuesta { get; set; }
    public string? DescripcionObra { get; set; }
    public decimal? ValorObra { get; set; }
    public decimal? PorcentajeUtilidad { get; set; }
    public decimal? Utilidad { get; set; }
    public decimal? PorcentajeIva { get; set; }
    public decimal? IvaUtilidad { get; set; }
    public decimal? Total { get; set; }
    public string? NotaPie { get; set; }
    public string? FirmaNombre { get; set; }
    public string? FirmaCargo { get; set; }
    public string? FirmaCelular { get; set; }
    public string? PlazoEntrega { get; set; }
    public int ShowPlazo { get; set; }
    public string? Garantia { get; set; }
    public int ShowGarantia { get; set; }
}

public class UpdateQuotationRequest
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string? Fecha { get; set; }
    public int? IdEdificio { get; set; }
    public string? Asunto { get; set; }
    public string? CordialSaludo { get; set; }
    public string? HeaderPropuesta { get; set; }
    public string? DescripcionObra { get; set; }
    public decimal? ValorObra { get; set; }
    public decimal? PorcentajeUtilidad { get; set; }
    public decimal? Utilidad { get; set; }
    public decimal? PorcentajeIva { get; set; }
    public decimal? IvaUtilidad { get; set; }
    public decimal? Total { get; set; }
    public string? NotaPie { get; set; }
    public string? FirmaNombre { get; set; }
    public string? FirmaCargo { get; set; }
    public string? FirmaCelular { get; set; }
    public string? PlazoEntrega { get; set; }
    public int ShowPlazo { get; set; }
    public string? Garantia { get; set; }
    public int ShowGarantia { get; set; }
}

public class QuotationResponse
{
    public int Id { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string? Fecha { get; set; }
    public int? IdEdificio { get; set; }
    public string? BuildingName { get; set; }
    public string? Asunto { get; set; }
    public string? CordialSaludo { get; set; }
    public string? HeaderPropuesta { get; set; }
    public string? DescripcionObra { get; set; }
    public decimal? ValorObra { get; set; }
    public decimal? PorcentajeUtilidad { get; set; }
    public decimal? Utilidad { get; set; }
    public decimal? PorcentajeIva { get; set; }
    public decimal? IvaUtilidad { get; set; }
    public decimal? Total { get; set; }
    public string? NotaPie { get; set; }
    public string? FirmaNombre { get; set; }
    public string? FirmaCargo { get; set; }
    public string? FirmaCelular { get; set; }
    public string? PlazoEntrega { get; set; }
    public int ShowPlazo { get; set; }
    public string? Garantia { get; set; }
    public int ShowGarantia { get; set; }
    public List<QuotationItemResponse>? QuotationItems { get; set; }
}
