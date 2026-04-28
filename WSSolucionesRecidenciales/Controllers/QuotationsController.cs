using Application.DTOs;
using Application.Features.Quotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WSSolucionesRecidenciales.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuotationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuotationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<QuotationResponse>>> GetAll()
    {
        var quotations = await _mediator.Send(new GetAllQuotationsQuery());
        return Ok(quotations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QuotationResponse>> GetById(int id)
    {
        var quotation = await _mediator.Send(new GetQuotationByIdQuery(id));

        if (quotation == null)
            return NotFound();

        return Ok(quotation);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateQuotationRequest request)
    {
        var id = await _mediator.Send(new CreateQuotationCommand(request));
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateQuotationRequest request)
    {
        if (id != request.Id)
            return BadRequest("El ID no coincide con el del cuerpo de la solicitud");

        var result = await _mediator.Send(new UpdateQuotationCommand(request));

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteQuotationCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }
}
