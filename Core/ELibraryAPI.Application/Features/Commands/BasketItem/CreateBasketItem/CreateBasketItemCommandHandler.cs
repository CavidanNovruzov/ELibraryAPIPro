using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Application.Features.Commands.BasketItem.CreateBasketItem;

public sealed class CreateBasketItemCommandHandler : IRequestHandler<CreateBasketItemCommandRequest, Result<CreateBasketItemCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBasketItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateBasketItemCommandResponse>> Handle(CreateBasketItemCommandRequest request, CancellationToken ct)
    {
        var basketItemReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.BasketItem, Guid>();
        var basketItemWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.BasketItem, Guid>();
        var productReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();

        var productInfo = await productReadRepo.GetAll(tracking: false)
            .Where(p => p.Id == request.ProductId)
            .Select(p => new
            {
                Exists = true,
                TotalStock = p.Stocks.Sum(s => s.Quantity) 
            })
            .FirstOrDefaultAsync(ct);

        if (productInfo == null)
            return Result<CreateBasketItemCommandResponse>.Failure("Product not found.");

        var existingItem = await basketItemReadRepo.GetSingleAsync(
            x => x.BasketId == request.BasketId && x.ProductId == request.ProductId,
            tracking: true, ct: ct);

        if (existingItem != null)
        {
            int totalDesiredQuantity = existingItem.Quantity + request.Quantity;

            if (productInfo.TotalStock < totalDesiredQuantity)
                return Result<CreateBasketItemCommandResponse>.Failure($"Insufficient stock. Available stock: {productInfo.TotalStock}");

            existingItem.Quantity = totalDesiredQuantity;
            basketItemWriteRepo.Update(existingItem);
        }
        else
        {
            if (productInfo.TotalStock < request.Quantity)
                return Result<CreateBasketItemCommandResponse>.Failure($"Only {productInfo.TotalStock} items available in stock.");

            existingItem = _mapper.Map<Domain.Entities.Concrete.BasketItem>(request);
            await basketItemWriteRepo.AddAsync(existingItem, ct);
        }

        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
            return Result<CreateBasketItemCommandResponse>.Success(new CreateBasketItemCommandResponse(existingItem.Id));

        return Result<CreateBasketItemCommandResponse>.Failure("An error occurred while adding the item to the basket.");
    }
}