using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.Permission.DeletePermission;

public sealed class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePermissionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletePermissionCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Auth.Permission, int>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Auth.Permission, int>();

        var permission = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (permission == null)
            return Result.Failure("Permission not found or already deleted.");

        permission.IsDeleted = true;
        writeRepo.Update(permission);
        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Permission moved to archive.");
    }
}
