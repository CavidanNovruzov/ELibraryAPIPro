using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUser.GetAllUsers;

/// <summary>Admin paneli üçün bütün istifadəçilərin siyahısı.</summary>
public sealed record GetAllUsersQueryRequest(int Page = 1, int Size = 20)
    : IRequest<Result<GetAllUsersQueryResponse>>;
