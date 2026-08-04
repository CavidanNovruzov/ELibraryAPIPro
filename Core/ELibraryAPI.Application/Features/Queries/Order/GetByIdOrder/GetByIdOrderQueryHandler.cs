using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Order.GetByIdOrder;

public sealed class GetByIdOrderQueryHandler : IRequestHandler<GetByIdOrderQueryRequest, Result<GetByIdOrderQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetByIdOrderQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetByIdOrderQueryResponse>> Handle(GetByIdOrderQueryRequest request, CancellationToken cancellationToken)
    {
        var orderData = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Order, Guid>()
            .GetAll(tracking: false)
            .Where(o => o.Id == request.Id)
            .Select(o => new
            {
                o.UserId,
                Detail = new OrderDetailDto(
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
                )
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (orderData == null)
            return Result<GetByIdOrderQueryResponse>.NotFound("Sifariş tapılmadı.");

        bool isOwner = orderData.UserId == _currentUserService.UserGuid;
        bool isAdmin = _currentUserService.IsAdmin; 

        if (!isOwner && !isAdmin)
            return Result<GetByIdOrderQueryResponse>.Forbidden("Bu sifarişə baxmaq üçün icazəniz yoxdur.");

        // 4. Uğurlu nəticə
        return Result<GetByIdOrderQueryResponse>.Success(new GetByIdOrderQueryResponse(orderData.Detail));
    }
}