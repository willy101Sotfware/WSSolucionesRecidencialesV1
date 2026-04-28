using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Companies;

// Queries
public record GetAllCompaniesQuery : IRequest<List<CompanyResponse>>;
public record GetCompanyByIdQuery(int Id) : IRequest<CompanyResponse?>;

// Commands
public record CreateCompanyCommand(CreateCompanyRequest Request) : IRequest<int>;
public record UpdateCompanyCommand(UpdateCompanyRequest Request) : IRequest<bool>;
public record DeleteCompanyCommand(int Id) : IRequest<bool>;

// Handler
public class CompaniesHandler :
    IRequestHandler<GetAllCompaniesQuery, List<CompanyResponse>>,
    IRequestHandler<GetCompanyByIdQuery, CompanyResponse?>,
    IRequestHandler<CreateCompanyCommand, int>,
    IRequestHandler<UpdateCompanyCommand, bool>,
    IRequestHandler<DeleteCompanyCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CompaniesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CompanyResponse>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companies = await _context.Companies
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CompanyResponse>>(companies);
    }

    public async Task<CompanyResponse?> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdEmpresa == request.Id, cancellationToken);

        return company == null ? null : _mapper.Map<CompanyResponse>(company);
    }

    public async Task<int> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = _mapper.Map<Company>(request.Request);
        company.Activo = 1;

        _context.Companies.Add(company);
        await _context.SaveChangesAsync(cancellationToken);

        return company.IdEmpresa;
    }

    public async Task<bool> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.IdEmpresa == request.Request.IdEmpresa, cancellationToken);

        if (company == null)
            return false;

        _mapper.Map(request.Request, company);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var company = await _context.Companies
            .FirstOrDefaultAsync(c => c.IdEmpresa == request.Id, cancellationToken);

        if (company == null)
            return false;

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
