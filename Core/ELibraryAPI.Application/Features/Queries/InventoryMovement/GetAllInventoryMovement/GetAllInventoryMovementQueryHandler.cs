using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Entities.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.InventoryMovement.GetAllInventoryMovement;

public sealed class GetAllInventoryMovementQueryHandler
    : IRequestHandler<GetAllInventoryMovementQueryRequest, Result<GetAllInventoryMovementQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllInventoryMovementQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<GetAllInventoryMovementQueryResponse>> Handle(
        GetAllInventoryMovementQueryRequest request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.InventoryMovement, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var movements = await query
            .OrderByDescending(im => im.CreatedDate)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(im => new InventoryMovementListDto(
                im.Id,
                im.ProductId,
                im.Product.Title,
                im.FromBranchId,
                im.FromBranch.Name,
                im.ToBranchId,
                im.ToBranch != null ? im.ToBranch.Name : null,
                im.Quantity,
                im.Type,
                im.Status,
                im.CreatedDate
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllInventoryMovementQueryResponse>.Success(
            new GetAllInventoryMovementQueryResponse(movements, totalCount, request.Page, request.Size, totalPages));
    }
}
