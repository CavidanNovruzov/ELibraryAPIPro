using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.Permission.UpdatePermission;

public sealed class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommandRequest, Result<UpdatePermissionCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePermissionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdatePermissionCommandResponse>> Handle(UpdatePermissionCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Auth.Permission, int>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Auth.Permission, int>();

        var permission = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (permission == null)
            return Result<UpdatePermissionCommandResponse>.Failure("İcazə tapılmadı.");

        var normalizedKey = request.Key.Trim();

        var keyExists = await readRepo.ExistsAsync(
            x => x.Id != request.Id && x.Key.ToLower() == normalizedKey.ToLower(),
            tracking: false,
            ct: ct);

        if (keyExists)
            return Result<UpdatePermissionCommandResponse>.Conflict("Bu açarla icazə artıq mövcuddur.");

        permission.Key = normalizedKey;
        writeRepo.Update(permission);
        await _unitOfWork.SaveAsync(ct);

        return Result<UpdatePermissionCommandResponse>.Success(
            new UpdatePermissionCommandResponse(permission.Id),
            "Əməliyyat uğurla tamamlandı.");
    }
}
