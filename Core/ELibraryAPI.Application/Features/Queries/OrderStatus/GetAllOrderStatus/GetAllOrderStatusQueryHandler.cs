using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;


namespace ELibraryAPI.Application.Features.Queries.OrderStatus.GetAllOrderStatus;

public sealed class GetAllOrderStatusQueryHandler : IRequestHandler<GetAllOrderStatusQueryRequest, Result<GetAllOrderStatusQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllOrderStatusQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllOrderStatusQueryResponse>> Handle(GetAllOrderStatusQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.OrderStatus, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var statuses = await query
            .OrderBy(s => s.Name)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(s => new OrderStatusListDto(s.Id, s.Name))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllOrderStatusQueryResponse>.Success(
            new GetAllOrderStatusQueryResponse(statuses, totalCount, request.Page, request.Size, totalPages));
    }
}