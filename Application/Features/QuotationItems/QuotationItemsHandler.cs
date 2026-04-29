using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.QuotationItems;

// Queries
public record GetAllQuotationItemsQuery : IRequest<List<QuotationItemResponse>>;
public record GetQuotationItemByIdQuery(int Id) : IRequest<QuotationItemResponse?>;
public record GetQuotationItemsByQuotationIdQuery(int QuotationId) : IRequest<List<QuotationItemResponse>>;

// Commands
public record CreateQuotationItemCommand(CreateQuotationItemRequest Request) : IRequest<int>;
public record UpdateQuotationItemCommand(UpdateQuotationItemRequest Request) : IRequest<bool>;
public record DeleteQuotationItemCommand(int Id) : IRequest<bool>;

// Handler
public class QuotationItemsHandler :
    IRequestHandler<GetAllQuotationItemsQuery, List<QuotationItemResponse>>,
    IRequestHandler<GetQuotationItemByIdQuery, QuotationItemResponse?>,
    IRequestHandler<GetQuotationItemsByQuotationIdQuery, List<QuotationItemResponse>>,
    IRequestHandler<CreateQuotationItemCommand, int>,
    IRequestHandler<UpdateQuotationItemCommand, bool>,
    IRequestHandler<DeleteQuotationItemCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public QuotationItemsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<QuotationItemResponse>> Handle(GetAllQuotationItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _context.QuotationItems
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<QuotationItemResponse>>(items);
    }

    public async Task<QuotationItemResponse?> Handle(GetQuotationItemByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _context.QuotationItems
            .AsNoTracking()
            .FirstOrDefaultAsync(qi => qi.Id == request.Id, cancellationToken);

        return item == null ? null : _mapper.Map<QuotationItemResponse>(item);
    }

    public async Task<List<QuotationItemResponse>> Handle(GetQuotationItemsByQuotationIdQuery request, CancellationToken cancellationToken)
    {
        var items = await _context.QuotationItems
            .AsNoTracking()
            .Where(qi => qi.IdCotizacion == request.QuotationId)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<QuotationItemResponse>>(items);
    }

    public async Task<int> Handle(CreateQuotationItemCommand request, CancellationToken cancellationToken)
    {
        var item = new QuotationItem
        {
            IdCotizacion = request.Request.IdCotizacion,
            Descripcion = request.Request.Descripcion ?? string.Empty,
            Cantidad = request.Request.Cantidad,
            UnidadMedida = request.Request.UnidadMedida,
            Imagen = request.Request.Imagen,
            ValorUnitario = request.Request.ValorUnitario,
            ValorTotal = request.Request.ValorTotal,
            PlazoEntrega = request.Request.PlazoEntrega,
            ShowPlazo = request.Request.ShowPlazo,
            Garantia = request.Request.Garantia,
            ShowGarantia = request.Request.ShowGarantia
        };

        _context.QuotationItems.Add(item);
        await _context.SaveChangesAsync(cancellationToken);

        return item.Id;
    }

    public async Task<bool> Handle(UpdateQuotationItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.QuotationItems
            .FirstOrDefaultAsync(qi => qi.Id == request.Request.Id, cancellationToken);

        if (item == null)
            return false;

        _mapper.Map(request.Request, item);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(DeleteQuotationItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _context.QuotationItems
            .FirstOrDefaultAsync(qi => qi.Id == request.Id, cancellationToken);

        if (item == null)
            return false;

        _context.QuotationItems.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
