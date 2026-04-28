using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Quotations;

// Queries
public record GetAllQuotationsQuery : IRequest<List<QuotationResponse>>;
public record GetQuotationByIdQuery(int Id) : IRequest<QuotationResponse?>;

// Commands
public record CreateQuotationCommand(CreateQuotationRequest Request) : IRequest<int>;
public record UpdateQuotationCommand(UpdateQuotationRequest Request) : IRequest<bool>;
public record DeleteQuotationCommand(int Id) : IRequest<bool>;

// Handler
public class QuotationsHandler :
    IRequestHandler<GetAllQuotationsQuery, List<QuotationResponse>>,
    IRequestHandler<GetQuotationByIdQuery, QuotationResponse?>,
    IRequestHandler<CreateQuotationCommand, int>,
    IRequestHandler<UpdateQuotationCommand, bool>,
    IRequestHandler<DeleteQuotationCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public QuotationsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<QuotationResponse>> Handle(GetAllQuotationsQuery request, CancellationToken cancellationToken)
    {
        var quotations = await _context.Quotations
            .AsNoTracking()
            .Include(q => q.Building)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<QuotationResponse>>(quotations);
    }

    public async Task<QuotationResponse?> Handle(GetQuotationByIdQuery request, CancellationToken cancellationToken)
    {
        var quotation = await _context.Quotations
            .AsNoTracking()
            .Include(q => q.Building)
            .FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

        return quotation == null ? null : _mapper.Map<QuotationResponse>(quotation);
    }

    public async Task<int> Handle(CreateQuotationCommand request, CancellationToken cancellationToken)
    {
        var quotation = _mapper.Map<Quotation>(request.Request);

        _context.Quotations.Add(quotation);
        await _context.SaveChangesAsync(cancellationToken);

        return quotation.Id;
    }

    public async Task<bool> Handle(UpdateQuotationCommand request, CancellationToken cancellationToken)
    {
        var quotation = await _context.Quotations
            .FirstOrDefaultAsync(q => q.Id == request.Request.Id, cancellationToken);

        if (quotation == null)
            return false;

        _mapper.Map(request.Request, quotation);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(DeleteQuotationCommand request, CancellationToken cancellationToken)
    {
        var quotation = await _context.Quotations
            .FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

        if (quotation == null)
            return false;

        _context.Quotations.Remove(quotation);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
