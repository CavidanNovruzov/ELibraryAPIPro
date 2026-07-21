using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.LogoutUser;

/// <summary>
/// İstifadəçinin aktiv refresh token-ini ləğv edərək sistemdən çıxarır.
/// UserId JWT claim-dən controller tərəfindən ötürülür.
/// </summary>
public sealed record LogoutUserCommandRequest(Guid UserId) : IRequest<Result>;
