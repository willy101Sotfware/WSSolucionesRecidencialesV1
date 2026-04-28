using Application.DTOs;
using Application.Features.Employees;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WSSolucionesRecidenciales.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponse>>> GetAll()
    {
        var employees = await _mediator.Send(new GetAllEmployeesQuery());
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeResponse>> GetById(int id)
    {
        var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));

        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateEmployeeRequest request)
    {
        var id = await _mediator.Send(new CreateEmployeeCommand(request));
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateEmployeeRequest request)
    {
        if (id != request.IdEmpleado)
            return BadRequest("El ID no coincide con el del cuerpo de la solicitud");

        var result = await _mediator.Send(new UpdateEmployeeCommand(request));

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteEmployeeCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }
}
