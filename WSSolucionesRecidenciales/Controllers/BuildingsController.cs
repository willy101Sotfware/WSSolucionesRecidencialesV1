using Application.DTOs;
using Application.Features.Buildings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WSSolucionesRecidenciales.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuildingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BuildingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<BuildingResponse>>> GetAll()
    {
        var buildings = await _mediator.Send(new GetAllBuildingsQuery());
        return Ok(buildings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BuildingResponse>> GetById(int id)
    {
        var building = await _mediator.Send(new GetBuildingByIdQuery(id));

        if (building == null)
            return NotFound();

        return Ok(building);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateBuildingRequest request)
    {
        var id = await _mediator.Send(new CreateBuildingCommand(request));
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateBuildingRequest request)
    {
        if (id != request.IdEdificio)
            return BadRequest("El ID no coincide con el del cuerpo de la solicitud");

        var result = await _mediator.Send(new UpdateBuildingCommand(request));

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteBuildingCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }
}
