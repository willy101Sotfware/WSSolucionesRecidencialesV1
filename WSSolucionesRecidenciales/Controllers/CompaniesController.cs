using Application.DTOs;
using Application.Features.Companies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WSSolucionesRecidenciales.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompaniesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CompanyResponse>>> GetAll()
    {
        var companies = await _mediator.Send(new GetAllCompaniesQuery());
        return Ok(companies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyResponse>> GetById(int id)
    {
        var company = await _mediator.Send(new GetCompanyByIdQuery(id));

        if (company == null)
            return NotFound();

        return Ok(company);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateCompanyRequest request)
    {
        var id = await _mediator.Send(new CreateCompanyCommand(request));
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateCompanyRequest request)
    {
        if (id != request.IdEmpresa)
            return BadRequest("El ID no coincide con el del cuerpo de la solicitud");

        var result = await _mediator.Send(new UpdateCompanyCommand(request));

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteCompanyCommand(id));

        if (!result)
            return NotFound();

        return NoContent();
    }
}
