using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Order.ChangeOrderStatus;

public sealed class ChangeOrderStatusCommandHandler : IRequestHandler<ChangeOrderStatusCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangeOrderStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ChangeOrderStatusCommandRequest request, CancellationToken ct)
    {
        var orderReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Order, Guid>();
        var orderWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Order, Guid>();

        var order = await orderReadRepo.GetByIdAsync(request.OrderId, tracking: true, ct: ct);

        if (order == null)
        {
            return Result.NotFound("Sifariş tapılmadı..");
        }

        order.OrderStatusId = request.StatusId;

        orderWriteRepo.Update(order);
        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
        {
            return Result.Success("Sifariş statusu uğurla yeniləndi.");
        }

        return Result.Failure("Sifariş statusu yenilənərkən xəta baş verdi.", ErrorType.ValidationError);
    }
}