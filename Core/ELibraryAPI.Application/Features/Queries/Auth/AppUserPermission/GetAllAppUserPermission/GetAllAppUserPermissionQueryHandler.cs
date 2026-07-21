using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppUserPermission.GetAllAppUserPermission;

public sealed class GetAllAppUserPermissionQueryHandler : IRequestHandler<GetAllAppUserPermissionQueryRequest, Result<List<GetAllAppUserPermissionQueryResponse>>>
{
    private readonly IUnitOfWork _uow;

    public GetAllAppUserPermissionQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Result<List<GetAllAppUserPermissionQueryResponse>>> Handle(GetAllAppUserPermissionQueryRequest request, CancellationToken ct)
    {
        var appUserPermissionRepository = _uow.ReadRepository<Domain.Entities.Concrete.Auth.AppUserPermission, Guid>();

        var permissions = await appUserPermissionRepository.GetAll(tracking: false)
            .Where(x => x.UserId == request.UserId)
            .Include(x => x.Permission)
            .Select(x => new GetAllAppUserPermissionQueryResponse(
                x.PermissionId,
                x.Permission.Key,
                x.CreatedDate
            ))
            .ToListAsync(ct);

        return Result<List<GetAllAppUserPermissionQueryResponse>>.Success(permissions);
    }
}