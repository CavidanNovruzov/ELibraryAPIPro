using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Order.GetOrderByNumber;

public sealed class GetOrderByNumberQueryHandler : IRequestHandler<GetOrderByNumberQueryRequest, Result<GetOrderByNumberQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrderByNumberQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetOrderByNumberQueryResponse>> Handle(GetOrderByNumberQueryRequest request, CancellationToken ct)
    {
        var orderReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Order, Guid>();

        var order = await orderReadRepo.GetAll(tracking: false)
            .Include(o => o.OrderStatus)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber.Trim(), ct);

        if (order == null)
            return Result<GetOrderByNumberQueryResponse>.NotFound("Sifariş tapılmadı..");

        var response = new GetOrderByNumberQueryResponse(
            order.Id,
            order.OrderNumber,
            order.TotalAmount,
            order.OrderStatus.Name,
            order.OrderNote,
            order.CreatedDate,
            order.OrderItems.Select(oi => new OrderItemDto(
                oi.ProductId,
                oi.Product.Title,
                oi.Quantity,
                oi.UnitPrice 
            )).ToList()
        );

        return Result<GetOrderByNumberQueryResponse>.Success(response);
    }
}