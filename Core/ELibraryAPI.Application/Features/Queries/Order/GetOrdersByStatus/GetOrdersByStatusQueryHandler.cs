using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Features.Queries.Order.GetAllOrder;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Order.GetOrdersByStatus;

public sealed class GetOrdersByStatusQueryHandler : IRequestHandler<GetOrdersByStatusQueryRequest, Result<GetOrdersByStatusQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetOrdersByStatusQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetOrdersByStatusQueryResponse>> Handle(GetOrdersByStatusQueryRequest request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAdmin)
            return Result<GetOrdersByStatusQueryResponse>.Forbidden("Bu əməliyyat yalnız inzibatçılar üçündür.");

        var orders = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Order, Guid>()
            .GetAll(tracking: false)
            .Where(o => o.OrderStatusId == request.StatusId)
            .OrderByDescending(o => o.CreatedDate)
            .Select(o => new OrderListDto(
                o.Id,
                o.OrderNumber,
                o.CreatedDate,
                o.TotalAmount,
                o.OrderStatus.Name,
                o.User.Email,
                o.OrderItems.Count
            ))
            .ToListAsync(cancellationToken);

        return Result<GetOrdersByStatusQueryResponse>.Success(new GetOrdersByStatusQueryResponse(orders));
    }
}