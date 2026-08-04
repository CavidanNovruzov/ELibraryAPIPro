using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.ChangeUserStatus;

public sealed class ChangeUserStatusCommandHandler(IUnitOfWork uow, ICacheService cacheService)
    : IRequestHandler<ChangeUserStatusCommandRequest, Result>
{
    public async Task<Result> Handle(ChangeUserStatusCommandRequest request, CancellationToken ct)
    {
        var user = await uow.ReadRepository<Domain.Entities.Concrete.Auth.AppUser, Guid>()
            .GetByIdAsync(request.Id, tracking: true, ct);

        if (user == null)
            return Result.NotFound("İstifadəçi tapılmadı.");

        user.IsActive = !user.IsActive;
        await uow.SaveAsync(ct);

        await cacheService.RemoveAsync($"user:profile:{request.Id}", ct);
        await cacheService.RemoveAsync($"user:detail:{request.Id}", ct);

        string statusText = user.IsActive ? "activated" : "deactivated";
        return Result.Success($"User status has been {statusText} uğurla tamamlandı.");
    }
}