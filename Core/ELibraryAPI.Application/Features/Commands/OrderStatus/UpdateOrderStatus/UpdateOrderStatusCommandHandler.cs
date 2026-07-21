using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.OrderStatus.UpdateOrderStatus;

public sealed class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommandRequest, Result<UpdateOrderStatusCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdateOrderStatusCommandResponse>> Handle(UpdateOrderStatusCommandRequest request, CancellationToken ct)
    {
        var orderStatusReadRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.OrderStatus, Guid>();
        var orderStatusWriteRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.OrderStatus, Guid>();

        var orderStatus = await orderStatusReadRepository.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (orderStatus == null)
        {
            return Result<UpdateOrderStatusCommandResponse>.Failure("Order status not found.");
        }

        if (orderStatus.Name.ToLower() != request.Name.Trim().ToLower())
        {
            var isDuplicate = await orderStatusReadRepository.ExistsAsync(
                x => x.Name.ToLower() == request.Name.Trim().ToLower() && x.Id != request.Id,
                tracking: false,
                ct: ct);

            if (isDuplicate)
            {
                return Result<UpdateOrderStatusCommandResponse>.Failure("Another order status with this name already exists.");
            }
        }

        _mapper.Map(request, orderStatus);

        orderStatusWriteRepository.Update(orderStatus);
        await _unitOfWork.SaveAsync(ct);

        return Result<UpdateOrderStatusCommandResponse>.Success(
            new UpdateOrderStatusCommandResponse(orderStatus.Id),
            "Order status updated successfully.");
    }
}