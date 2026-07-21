using ELibraryAPI.Application.Abstractions; // IUnitOfWork üçün
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Permission.GetAllPermission;

public sealed class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQueryRequest, Result<List<GetAllPermissionQueryResponse>>>
{
    private readonly IUnitOfWork _uow;

    public GetAllPermissionQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<List<GetAllPermissionQueryResponse>>> Handle(GetAllPermissionQueryRequest request, CancellationToken ct)
    {
        var permissions = await _uow.ReadRepository<Domain.Entities.Concrete.Auth.Permission, int>().GetAll(tracking: false)
            .OrderByDescending(x => x.CreatedDate)
            .Select(x => new GetAllPermissionQueryResponse(
                x.Id,
                x.Key,
                x.IsDelegatable,
                x.CreatedDate
            ))
            .ToListAsync(ct);

        return Result<List<GetAllPermissionQueryResponse>>.Success(permissions);
    }
}