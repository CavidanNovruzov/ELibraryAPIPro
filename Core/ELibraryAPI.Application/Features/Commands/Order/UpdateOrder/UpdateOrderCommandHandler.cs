using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Order.UpdateOrder;

public sealed class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommandRequest, Result<UpdateOrderCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdateOrderCommandResponse>> Handle(UpdateOrderCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Order, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Order, Guid>();

        var order = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (order == null)
        {
            return Result<UpdateOrderCommandResponse>.Failure("Order not found.");
        }

        order.OrderNote = request.OrderNote;
        order.OrderStatusId = request.OrderStatusId;
        order.PaymentMethodId = request.PaymentMethodId;
        order.ShippingMethodId = request.ShippingMethodId;

        writeRepo.Update(order);
        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
        {
            return Result<UpdateOrderCommandResponse>.Success(
                new UpdateOrderCommandResponse(order.Id),
                "Order status and information updated successfully.");
        }

        return Result<UpdateOrderCommandResponse>.Failure("No changes were applied to the order.");
    }
}