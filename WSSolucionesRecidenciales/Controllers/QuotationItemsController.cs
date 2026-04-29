using Application.DTOs;
using Application.Features.QuotationItems;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace WSSolucionesRecidenciales.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuotationItemsController : ControllerBase
{
    private readonly IConfiguration _config;

    public QuotationItemsController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public async Task<ActionResult<List<QuotationItemResponse>>> GetAll()
    {
        try
        {
            var items = new List<QuotationItemResponse>();
            
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();
            
            using var cmd = new SqlCommand("SELECT Id, IdCotizacion, Descripcion, Cantidad, UnidadMedida, Imagen, ValorUnitario, ValorTotal, PlazoEntrega, ShowPlazo, Garantia, ShowGarantia FROM QuotationItems", connection);
            using var reader = await cmd.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                items.Add(new QuotationItemResponse
                {
                    Id = reader.GetInt32(0),
                    IdCotizacion = reader.GetInt32(1),
                    Descripcion = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Cantidad = reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                    UnidadMedida = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Imagen = reader.IsDBNull(5) ? null : reader.GetString(5),
                    ValorUnitario = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                    ValorTotal = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                    PlazoEntrega = reader.IsDBNull(8) ? null : reader.GetString(8),
                    ShowPlazo = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                    Garantia = reader.IsDBNull(10) ? null : reader.GetString(10),
                    ShowGarantia = reader.IsDBNull(11) ? null : reader.GetInt32(11)
                });
            }
            
            return Ok(items);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateQuotationItemRequest request)
    {
        try
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();
            
            var sql = @"INSERT INTO QuotationItems (IdCotizacion, Descripcion, Cantidad, UnidadMedida, Imagen, ValorUnitario, ValorTotal, PlazoEntrega, ShowPlazo, Garantia, ShowGarantia) 
                       VALUES (@IdCotizacion, @Descripcion, @Cantidad, @UnidadMedida, @Imagen, @ValorUnitario, @ValorTotal, @PlazoEntrega, @ShowPlazo, @Garantia, @ShowGarantia);
                       SELECT CAST(SCOPE_IDENTITY() as int);";
            
            using var cmd = new SqlCommand(sql, connection);
            
            cmd.Parameters.AddWithValue("@IdCotizacion", request.IdCotizacion);
            cmd.Parameters.AddWithValue("@Descripcion", request.Descripcion ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Cantidad", request.Cantidad ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@UnidadMedida", request.UnidadMedida ?? (object)DBNull.Value);
            
            if (string.IsNullOrEmpty(request.Imagen))
                cmd.Parameters.AddWithValue("@Imagen", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@Imagen", request.Imagen);
            
            cmd.Parameters.AddWithValue("@ValorUnitario", request.ValorUnitario ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ValorTotal", request.ValorTotal ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PlazoEntrega", request.PlazoEntrega ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ShowPlazo", request.ShowPlazo ?? 0);
            cmd.Parameters.AddWithValue("@Garantia", request.Garantia ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ShowGarantia", request.ShowGarantia ?? 0);
            
            var result = await cmd.ExecuteScalarAsync();
            var id = Convert.ToInt32(result);
            
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, inner = ex.InnerException?.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QuotationItemResponse>> GetById(int id)
    {
        using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();
        
        using var cmd = new SqlCommand("SELECT Id, IdCotizacion, Descripcion, Cantidad, UnidadMedida, Imagen, ValorUnitario, ValorTotal, PlazoEntrega, ShowPlazo, Garantia, ShowGarantia FROM QuotationItems WHERE Id = @Id", connection);
        cmd.Parameters.AddWithValue("@Id", id);
        
        using var reader = await cmd.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            return Ok(new QuotationItemResponse
            {
                Id = reader.GetInt32(0),
                IdCotizacion = reader.GetInt32(1),
                Descripcion = reader.IsDBNull(2) ? "" : reader.GetString(2),
                Cantidad = reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                UnidadMedida = reader.IsDBNull(4) ? null : reader.GetString(4),
                Imagen = reader.IsDBNull(5) ? null : reader.GetString(5),
                ValorUnitario = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                ValorTotal = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                PlazoEntrega = reader.IsDBNull(8) ? null : reader.GetString(8),
                ShowPlazo = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                Garantia = reader.IsDBNull(10) ? null : reader.GetString(10),
                ShowGarantia = reader.IsDBNull(11) ? null : reader.GetInt32(11)
            });
        }
        
        return NotFound();
    }

    [HttpGet("by-quotation/{quotationId}")]
    public async Task<ActionResult<List<QuotationItemResponse>>> GetByQuotationId(int quotationId)
    {
        try
        {
            var items = new List<QuotationItemResponse>();
            
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();
            
            using var cmd = new SqlCommand("SELECT Id, IdCotizacion, Descripcion, Cantidad, UnidadMedida, Imagen, ValorUnitario, ValorTotal, PlazoEntrega, ShowPlazo, Garantia, ShowGarantia FROM QuotationItems WHERE IdCotizacion = @IdCotizacion", connection);
            cmd.Parameters.AddWithValue("@IdCotizacion", quotationId);
            
            using var reader = await cmd.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                items.Add(new QuotationItemResponse
                {
                    Id = reader.GetInt32(0),
                    IdCotizacion = reader.GetInt32(1),
                    Descripcion = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Cantidad = reader.IsDBNull(3) ? null : reader.GetDecimal(3),
                    UnidadMedida = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Imagen = reader.IsDBNull(5) ? null : reader.GetString(5),
                    ValorUnitario = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                    ValorTotal = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                    PlazoEntrega = reader.IsDBNull(8) ? null : reader.GetString(8),
                    ShowPlazo = reader.IsDBNull(9) ? null : reader.GetInt32(9),
                    Garantia = reader.IsDBNull(10) ? null : reader.GetString(10),
                    ShowGarantia = reader.IsDBNull(11) ? null : reader.GetInt32(11)
                });
            }
            
            return Ok(items);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}