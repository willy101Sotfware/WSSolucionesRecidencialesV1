using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Buildings;

// Queries
public record GetAllBuildingsQuery : IRequest<List<BuildingResponse>>;
public record GetBuildingByIdQuery(int Id) : IRequest<BuildingResponse?>;

// Commands
public record CreateBuildingCommand(CreateBuildingRequest Request) : IRequest<int>;
public record UpdateBuildingCommand(UpdateBuildingRequest Request) : IRequest<bool>;
public record DeleteBuildingCommand(int Id) : IRequest<bool>;

// Handler
public class BuildingsHandler :
    IRequestHandler<GetAllBuildingsQuery, List<BuildingResponse>>,
    IRequestHandler<GetBuildingByIdQuery, BuildingResponse?>,
    IRequestHandler<CreateBuildingCommand, int>,
    IRequestHandler<UpdateBuildingCommand, bool>,
    IRequestHandler<DeleteBuildingCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public BuildingsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BuildingResponse>> Handle(GetAllBuildingsQuery request, CancellationToken cancellationToken)
    {
        var buildings = await _context.Buildings
            .AsNoTracking()
            .Include(b => b.Company)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<BuildingResponse>>(buildings);
    }

    public async Task<BuildingResponse?> Handle(GetBuildingByIdQuery request, CancellationToken cancellationToken)
    {
        var building = await _context.Buildings
            .AsNoTracking()
            .Include(b => b.Company)
            .FirstOrDefaultAsync(b => b.IdEdificio == request.Id, cancellationToken);

        return building == null ? null : _mapper.Map<BuildingResponse>(building);
    }

    public async Task<int> Handle(CreateBuildingCommand request, CancellationToken cancellationToken)
    {
        var building = _mapper.Map<Building>(request.Request);
        building.Activo = 1;

        _context.Buildings.Add(building);
        await _context.SaveChangesAsync(cancellationToken);

        return building.IdEdificio;
    }

    public async Task<bool> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
    {
        var building = await _context.Buildings
            .FirstOrDefaultAsync(b => b.IdEdificio == request.Request.IdEdificio, cancellationToken);

        if (building == null)
            return false;

        _mapper.Map(request.Request, building);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(DeleteBuildingCommand request, CancellationToken cancellationToken)
    {
        var building = await _context.Buildings
            .FirstOrDefaultAsync(b => b.IdEdificio == request.Id, cancellationToken);

        if (building == null)
            return false;

        _context.Buildings.Remove(building);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
