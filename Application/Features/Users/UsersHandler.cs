using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users;

// Queries
public record GetAllUsersQuery : IRequest<List<UserResponse>>;
public record GetUserByIdQuery(int Id) : IRequest<UserResponse?>;
public record LoginQuery(string Username, string Password) : IRequest<LoginResponse?>;

// Commands
public record CreateUserCommand(CreateUserRequest Request) : IRequest<int>;
public record UpdateUserCommand(UpdateUserRequest Request) : IRequest<bool>;
public record DeleteUserCommand(int Id) : IRequest<bool>;

// Handler
public class UsersHandler :
    IRequestHandler<GetAllUsersQuery, List<UserResponse>>,
    IRequestHandler<GetUserByIdQuery, UserResponse?>,
    IRequestHandler<LoginQuery, LoginResponse?>,
    IRequestHandler<CreateUserCommand, int>,
    IRequestHandler<UpdateUserCommand, bool>,
    IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UsersHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<UserResponse>>(users);
    }

    public async Task<UserResponse?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        return user == null ? null : _mapper.Map<UserResponse>(user);
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request.Request);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Request.Id, cancellationToken);

        if (user == null)
            return false;

        _mapper.Map(request.Request, user);

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<LoginResponse?> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password, cancellationToken);

        if (user == null)
            return null;

        // Generar token simple (en producción usar JWT)
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        return new LoginResponse
        {
            Token = token,
            User = _mapper.Map<UserResponse>(user)
        };
    }
}
