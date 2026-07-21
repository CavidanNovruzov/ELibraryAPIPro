using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUserPermission.GetByIdAppUserPermission;

public sealed class GetByIdAppUserPermissionQueryHandler : IRequestHandler<GetByIdAppUserPermissionQueryRequest, Result<GetByIdAppUserPermissionQueryResponse>>
{
    private readonly IUnitOfWork _uow;

    public GetByIdAppUserPermissionQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Result<GetByIdAppUserPermissionQueryResponse>> Handle(GetByIdAppUserPermissionQueryRequest request, CancellationToken ct)
    {
        var appUserPermissionRepository = _uow.ReadRepository<Domain.Entities.Concrete.Auth.AppUserPermission, Guid>();

        var permission = await appUserPermissionRepository.GetAll(tracking: false)
            .Include(x => x.Permission)
            .Where(x => x.UserId == request.UserId && x.PermissionId == request.PermissionId)
            .Select(x => new GetByIdAppUserPermissionQueryResponse(
                x.PermissionId,
                x.Permission.Key,
                x.CreatedDate
            ))
            .FirstOrDefaultAsync(ct);

        if (permission == null)
            return Result<GetByIdAppUserPermissionQueryResponse>.Failure("User permission not found.");

        return Result<GetByIdAppUserPermissionQueryResponse>.Success(permission);
    }
}