using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Order.GetOrderByNumber;

public sealed class GetOrderByNumberQueryHandler : IRequestHandler<GetOrderByNumberQueryRequest, Result<GetOrderByNumberQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public GetOrderByNumberQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetOrderByNumberQueryResponse>> Handle(GetOrderByNumberQueryRequest request, CancellationToken ct)
    {
        var order = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Order, Guid>()
            .GetAll(tracking: false)
            .Include(o => o.OrderStatus)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderNumber == request.OrderNumber.Trim(), ct);

        if (order == null)
            return Result<GetOrderByNumberQueryResponse>.NotFound("Sifariş tapılmadı.");

        if (order.UserId != _currentUserService.UserGuid && !_currentUserService.IsAdmin)
            return Result<GetOrderByNumberQueryResponse>.Forbidden("Bu sifarişə baxmaq üçün icazəniz yoxdur.");

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