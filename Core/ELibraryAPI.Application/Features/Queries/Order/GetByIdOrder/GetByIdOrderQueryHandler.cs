using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Order.GetByIdOrder;

public sealed class GetByIdOrderQueryHandler : IRequestHandler<GetByIdOrderQueryRequest, Result<GetByIdOrderQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdOrderQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetByIdOrderQueryResponse>> Handle(GetByIdOrderQueryRequest request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Order, Guid>()
            .GetAll(tracking: false)
            .Where(o => o.Id == request.Id)
            .Select(o => new OrderDetailDto(
                o.Id,
                o.OrderNumber,
                o.CreatedDate,
                o.TotalAmount,
                o.OrderNote ?? string.Empty,
                o.OrderStatus.Name,
                o.PaymentMethod.Name,
                o.ShippingMethod.Name,
                o.User.Email,
                o.User.PhoneNumber ?? string.Empty,
                o.OrderItems.Select(oi => new OrderItemDetailDto(
                    oi.Id,
                    oi.ProductId,
                    oi.Product.Title,
                    oi.Quantity,
                    oi.UnitPrice,
                    oi.UnitPrice * oi.Quantity
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (order == null)
            return Result<GetByIdOrderQueryResponse>.Failure("Order not found");

        return Result<GetByIdOrderQueryResponse>.Success(new GetByIdOrderQueryResponse(order));
    }
}