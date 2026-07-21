using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.Permission.CreatePermission;

public sealed class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommandRequest, Result<CreatePermissionCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreatePermissionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreatePermissionCommandResponse>> Handle(CreatePermissionCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Auth.Permission, int>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Auth.Permission, int>();

        var normalizedKey = request.Key.Trim();

        var keyExists = await readRepo.ExistsAsync(
            x => x.Key.ToLower() == normalizedKey.ToLower(),
            tracking: false,
            ct: ct);

        if (keyExists)
            return Result<CreatePermissionCommandResponse>.Failure("A permission with this key already exists.");

        var permission = new Domain.Entities.Concrete.Auth.Permission
        {
            Key = normalizedKey
        };

        await writeRepo.AddAsync(permission, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreatePermissionCommandResponse>.Success(
            new CreatePermissionCommandResponse(permission.Id),
            "Permission created successfully.");
    }
}
