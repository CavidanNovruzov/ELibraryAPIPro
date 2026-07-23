using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.BasketItem.ClearBasketItem;

public sealed class ClearBasketItemCommandHandler : IRequestHandler<ClearBasketItemCommandRequest, Result<ClearBasketItemCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ClearBasketItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ClearBasketItemCommandResponse>> Handle(ClearBasketItemCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.BasketItem, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.BasketItem, Guid>();

        var items = await readRepo.GetWhere(x => x.BasketId == request.BasketId, tracking: true).ToListAsync();

        if (items.Count == 0)
            return Result<ClearBasketItemCommandResponse>.Success(new ClearBasketItemCommandResponse());

        writeRepo.RemoveRange(items);

        var result = await _unitOfWork.SaveAsync(ct);

        if (result > 0)
            return Result<ClearBasketItemCommandResponse>.Success(new ClearBasketItemCommandResponse());

        return Result<ClearBasketItemCommandResponse>.Failure("Səbət elementləri təmizlənərkən xəta baş verdi.");
    }
}