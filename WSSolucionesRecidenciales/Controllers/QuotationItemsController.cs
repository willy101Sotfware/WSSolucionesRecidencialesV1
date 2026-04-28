using Application.DTOs;
using Application.Features.QuotationItems;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WSSolucionesRecidenciales.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuotationItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuotationItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<QuotationItemResponse>>> GetAll()
    {
        var items = await _mediator.Send(new GetAllQuotationItemsQuery());
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QuotationItemResponse>> GetById(int id)
    {
        var item = await _mediator.Send(new GetQuotationItemByIdQuery(id));

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpGet("by-quotation/{quotationId}")]
    public async Task<ActionResult<List<QuotationItemResponse>>> GetByQuotationId(int quotationId)
    {
        var items = await _mediator.Send(new GetQuotationItemsByQuotationIdQuery(quotationId));
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateQuotationItemRequest request)
    {
        var id = await _mediator.Send(new CreateQuotationItemCommand(request));
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateQuotationItemRequest request)
    {
        if (id != request.Id)
            return BadRequest("El ID no coincide con el del cuerpo de la solicitud");

        var result = await _mediator.Send(new UpdateQuotationItemCommand(request));

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteQuotationItemCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }
}
