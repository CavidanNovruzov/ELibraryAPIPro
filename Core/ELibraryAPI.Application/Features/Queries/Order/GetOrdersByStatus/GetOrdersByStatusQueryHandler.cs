using ELibraryAPI.Application.Features.Queries.Order.GetAllOrder;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Application.Features.Queries.Order.GetOrdersByStatus;

public sealed class GetOrdersByStatusQueryHandler : IRequestHandler<GetOrdersByStatusQueryRequest, Result<GetOrdersByStatusQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrdersByStatusQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetOrdersByStatusQueryResponse>> Handle(GetOrdersByStatusQueryRequest request, CancellationToken cancellationToken)
    {
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
