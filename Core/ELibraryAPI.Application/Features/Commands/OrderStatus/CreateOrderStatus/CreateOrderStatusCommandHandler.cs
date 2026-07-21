using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.OrderStatus.CreateOrderStatus;

public sealed class CreateOrderStatusCommandHandler : IRequestHandler<CreateOrderStatusCommandRequest, Result<CreateOrderStatusCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrderStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateOrderStatusCommandResponse>> Handle(CreateOrderStatusCommandRequest request, CancellationToken ct)
    {
        var orderStatusReadRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.OrderStatus, Guid>();
        var orderStatusWriteRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.OrderStatus, Guid>();

        var isExists = await orderStatusReadRepository.ExistsAsync(
            x => x.Name.ToLower() == request.Name.Trim().ToLower(),
            tracking: false,
            ct: ct);

        if (isExists)
        {
            return Result<CreateOrderStatusCommandResponse>.Failure("This order status already exists.");
        }

        var orderStatus = _mapper.Map<Domain.Entities.Concrete.OrderStatus>(request);

        await orderStatusWriteRepository.AddAsync(orderStatus, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateOrderStatusCommandResponse>.Success(
            new CreateOrderStatusCommandResponse(orderStatus.Id),
            "Order status created successfully.");
    }
}