using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Employees;

// Queries
public record GetAllEmployeesQuery : IRequest<List<EmployeeResponse>>;
public record GetEmployeeByIdQuery(int Id) : IRequest<EmployeeResponse?>;

// Commands
public record CreateEmployeeCommand(CreateEmployeeRequest Request) : IRequest<int>;
public record UpdateEmployeeCommand(UpdateEmployeeRequest Request) : IRequest<bool>;
public record DeleteEmployeeCommand(int Id) : IRequest<bool>;

// Handler
public class EmployeesHandler :
    IRequestHandler<GetAllEmployeesQuery, List<EmployeeResponse>>,
    IRequestHandler<GetEmployeeByIdQuery, EmployeeResponse?>,
    IRequestHandler<CreateEmployeeCommand, int>,
    IRequestHandler<UpdateEmployeeCommand, bool>,
    IRequestHandler<DeleteEmployeeCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EmployeesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<EmployeeResponse>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _context.Employees
            .AsNoTracking()
            .Include(e => e.Building)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<EmployeeResponse>>(employees);
    }

    public async Task<EmployeeResponse?> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .AsNoTracking()
            .Include(e => e.Building)
            .FirstOrDefaultAsync(e => e.IdEmpleado == request.Id, cancellationToken);

        return employee == null ? null : _mapper.Map<EmployeeResponse>(employee);
    }

    public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = _mapper.Map<Employee>(request.Request);
        employee.Activo = 1;

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync(cancellationToken);

        return employee.IdEmpleado;
    }

    public async Task<bool> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.IdEmpleado == request.Request.IdEmpleado, cancellationToken);

        if (employee == null)
            return false;

        _mapper.Map(request.Request, employee);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.IdEmpleado == request.Id, cancellationToken);

        if (employee == null)
            return false;

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
