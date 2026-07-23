using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.Auth.LogoutUser;

public sealed class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public LogoutUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(LogoutUserCommandRequest request, CancellationToken ct)
    {
        var affectedRows = await _unitOfWork.RefreshTokenRead.GetAll()
            .Where(x => x.UserId == request.UserId && !x.IsRevoked)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(t => t.IsRevoked, true)
                .SetProperty(t => t.RevokedAt, DateTime.UtcNow), 
                ct);

        if (affectedRows == 0)
        {
            return Result.Success("Aktiv sessiya tapılmadı və ya artıq çıxış edilib.");
        }

        return Result.Success("Sistemdən uğurla çıxış edildi.");
    }
}