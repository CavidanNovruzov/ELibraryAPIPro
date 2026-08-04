using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Models;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Order.ChangeOrderStatus;

public sealed class ChangeOrderStatusCommandHandler : IRequestHandler<ChangeOrderStatusCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventBus _eventBus;
    public ChangeOrderStatusCommandHandler(IUnitOfWork unitOfWork, IEventBus eventBus)
    {
        _unitOfWork = unitOfWork;
        _eventBus = eventBus;
    }

    public async Task<Result> Handle(ChangeOrderStatusCommandRequest request, CancellationToken ct)
    {
        var orderReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Order, Guid>();
        var orderWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Order, Guid>();

        var order = await orderReadRepo.GetByIdAsync(request.OrderId, tracking: true, ct: ct);
        if (order == null)
            return Result.NotFound("Sifariş tapılmadı.");

        var newStatus = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.OrderStatus, Guid>()
            .GetByIdAsync(request.StatusId, tracking: false, ct: ct);
        if (newStatus == null)
            return Result.NotFound("Status tapılmadı.");

        order.OrderStatusId = request.StatusId;
        orderWriteRepo.Update(order);

        var result = await _unitOfWork.SaveAsync(ct);
        if (result <= 0)
            return Result.Failure("Sifariş statusu yenilənərkən xəta baş verdi.", ErrorType.ValidationError);

        await _eventBus.PublishAsync(new OrderStatusChangedMessage(order.Id, newStatus.Name), ct);

        return Result.Success("Sifariş statusu uğurla yeniləndi.");
    }
}