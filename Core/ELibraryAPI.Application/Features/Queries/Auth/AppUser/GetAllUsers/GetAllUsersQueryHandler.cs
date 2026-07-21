using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetAllUsers;

public sealed class GetAllUsersQueryHandler
    : IRequestHandler<GetAllUsersQueryRequest, Result<GetAllUsersQueryResponse>>
{
    private readonly UserManager<Domain.Entities.Concrete.Auth.AppUser> _userManager;

    public GetAllUsersQueryHandler(UserManager<Domain.Entities.Concrete.Auth.AppUser> userManager)
        => _userManager = userManager;

    public async Task<Result<GetAllUsersQueryResponse>> Handle(
        GetAllUsersQueryRequest request, CancellationToken ct)
    {
        var query = _userManager.Users.AsNoTracking();
        var total = await query.CountAsync(ct);

        var users = await query
            .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(u => new UserSummaryDto(
                u.Id, $"{u.FirstName} {u.LastName}", u.Email!, u.UserName!, u.EmailConfirmed))
            .ToListAsync(ct);

        return Result<GetAllUsersQueryResponse>.Success(new(users, total));
    }
}
