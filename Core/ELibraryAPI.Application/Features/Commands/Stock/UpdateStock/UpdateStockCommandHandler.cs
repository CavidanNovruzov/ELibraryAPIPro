using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.Stock.UpdateStock;

public sealed class UpdateStockCommandHandler
    : IRequestHandler<UpdateStockCommandRequest, Result<UpdateStockCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UpdateStockCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Result<UpdateStockCommandResponse>> Handle(
        UpdateStockCommandRequest request,
        CancellationToken ct)
    {
        var stockReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Stock, Guid>();
        var productReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();
        var branchReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Branch, Guid>();

        var stock = await stockReadRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (stock == null)
            return Result<UpdateStockCommandResponse>.Failure("Stok qeydi tapılmadı.");

        if (!await productReadRepo.ExistsAsync(x => x.Id == request.ProductId, false, ct))
            return Result<UpdateStockCommandResponse>.Failure("Məhsul tapılmadı..");

        if (!await branchReadRepo.ExistsAsync(x => x.Id == request.BranchId, false, ct))
            return Result<UpdateStockCommandResponse>.Failure("Filial tapılmadı.");

        if (stock.ProductId != request.ProductId || stock.BranchId != request.BranchId)
        {
            var stockExists = await stockReadRepo.ExistsAsync(
                x => x.ProductId == request.ProductId && x.BranchId == request.BranchId && x.Id != request.Id,
                false, ct);

            if (stockExists)
                return Result<UpdateStockCommandResponse>.Failure(
                    "Seçilmiş filialda bu məhsul üçün stok qeydi artıq mövcuddur.");
        }

        if (request.Quantity < 0)
            return Result<UpdateStockCommandResponse>.Failure("Stok miqdarı mənfi ola bilməz.");

        int previousQuantity = stock.Quantity;

        _mapper.Map(request, stock);

        try
        {
            await _unitOfWork.SaveAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result<UpdateStockCommandResponse>.Failure(
                "Stok başqa bir sorğu tərəfindən dəyişdirildi. Zəhmət olmasa səhifəni yeniləyin və yenidən cəhd edin.",
                ErrorType.Conflict);
        }

        if (previousQuantity == 0 && request.Quantity > 0)
        {
            await _mediator.Publish(new ProductBackInStockEvent(stock.ProductId), ct);
        }


        return Result<UpdateStockCommandResponse>.Success(
            new UpdateStockCommandResponse(stock.Id),
            "Əməliyyat uğurla tamamlandı.");
    }
}