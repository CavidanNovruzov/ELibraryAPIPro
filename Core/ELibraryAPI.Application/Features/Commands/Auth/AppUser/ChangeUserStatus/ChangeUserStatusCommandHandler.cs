using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.ChangeUserStatus;

public sealed class ChangeUserStatusCommandHandler(IUnitOfWork uow)
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

        string statusText = user.IsActive ? "activated" : "deactivated";
        return Result.Success($"User status has been {statusText} uğurla tamamlandı.");
    }
}
