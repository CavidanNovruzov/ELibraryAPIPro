
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;


namespace ELibraryAPI.Application.Features.Queries.Permission.GetByIdPermission;

public sealed class GetByIdPermissionQueryHandler : IRequestHandler<GetByIdPermissionQueryRequest, Result<GetByIdPermissionQueryResponse>>
{
    private readonly IUnitOfWork _uow;

    public GetByIdPermissionQueryHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }
    public async Task<Result<GetByIdPermissionQueryResponse>> Handle(GetByIdPermissionQueryRequest request, CancellationToken ct)
    {

        var permission = await _uow.ReadRepository<Domain.Entities.Concrete.Auth.Permission, int>()
            .GetByIdAsync(request.Id, tracking: false, ct);

           if (permission == null)
            return Result<GetByIdPermissionQueryResponse>.NotFound("Permission not found.");

        var response = new GetByIdPermissionQueryResponse(
            permission.Id,
            permission.Key,
            permission.IsDelegatable,
            permission.CreatedDate
        );

        return Result<GetByIdPermissionQueryResponse>.Success(response);
    }
}