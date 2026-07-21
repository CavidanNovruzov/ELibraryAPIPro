using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Order.GetMyOrders;

public sealed class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQueryRequest, Result<GetMyOrdersQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetMyOrdersQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetMyOrdersQueryResponse>> Handle(GetMyOrdersQueryRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;

        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Order, Guid>()
            .GetAll(tracking: false)
            .Include(o => o.OrderStatus)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.CreatedDate);

        var totalCount = await query.CountAsync(ct);


        var orders = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(o => new MyOrderDto(
                o.Id,
                o.OrderNumber,
                o.TotalAmount,
                o.OrderStatus.Name,
                o.CreatedDate,
                o.OrderItems.Count
            ))
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetMyOrdersQueryResponse>.Success(
            new GetMyOrdersQueryResponse(orders, totalCount, request.Page, request.Size, totalPages));
    }
}