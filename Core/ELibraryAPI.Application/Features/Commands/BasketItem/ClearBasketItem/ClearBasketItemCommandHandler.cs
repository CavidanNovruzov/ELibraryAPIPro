using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.BasketItem.ClearBasketItem;

public sealed class ClearBasketItemCommandHandler : IRequestHandler<ClearBasketItemCommandRequest, Result<ClearBasketItemCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ClearBasketItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ClearBasketItemCommandResponse>> Handle(ClearBasketItemCommandRequest request, CancellationToken ct)
    {
        var basket = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Basket, Guid>()
            .GetByIdAsync(request.BasketId, tracking: false, ct: ct);

        if (basket == null)
            return Result<ClearBasketItemCommandResponse>.NotFound("Səbət tapılmadı.");

        if (basket.UserId != _currentUserService.UserGuid && !_currentUserService.IsAdmin)
            return Result<ClearBasketItemCommandResponse>.Forbidden("Bu səbəti təmizləmək üçün icazəniz yoxdur.");

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