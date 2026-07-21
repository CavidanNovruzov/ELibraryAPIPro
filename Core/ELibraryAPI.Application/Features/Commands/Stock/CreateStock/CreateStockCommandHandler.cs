using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Stock.CreateStock;

public sealed class CreateStockCommandHandler : IRequestHandler<CreateStockCommandRequest, Result<CreateStockCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreateStockCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Result<CreateStockCommandResponse>> Handle(CreateStockCommandRequest request, CancellationToken ct)
    {
        var stockReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Stock, Guid>();
        var stockWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Stock, Guid>();
        var productReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();
        var branchReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Branch, Guid>();

        var productExists = await productReadRepo.ExistsAsync(x => x.Id == request.ProductId, false, ct);
        if (!productExists)
            return Result<CreateStockCommandResponse>.Failure("Product not found.");

        var branchExists = await branchReadRepo.ExistsAsync(x => x.Id == request.BranchId, false, ct);
        if (!branchExists)
            return Result<CreateStockCommandResponse>.Failure("Branch not found.");


        var stockAlreadyExists = await stockReadRepo.ExistsAsync(
            x => x.ProductId == request.ProductId && x.BranchId == request.BranchId,
            false,
            ct);

        if (stockAlreadyExists)
            return Result<CreateStockCommandResponse>.Failure("This product already has a stock record in the selected branch.");

        var stock = _mapper.Map<Domain.Entities.Concrete.Stock>(request);

        await stockWriteRepo.AddAsync(stock, ct);

        await _unitOfWork.SaveAsync(ct);

        if (request.Quantity > 0)
        {
            await _mediator.Publish(new ProductBackInStockEvent(stock.ProductId), ct);
        }

        return Result<CreateStockCommandResponse>.Success(
            new CreateStockCommandResponse(stock.Id),
            "Stock record created successfully.");
    }
}