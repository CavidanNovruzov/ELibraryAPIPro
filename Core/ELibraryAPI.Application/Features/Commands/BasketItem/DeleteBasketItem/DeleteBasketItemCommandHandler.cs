using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.BasketItem.DeleteBasketItem;

public sealed class DeleteBasketItemCommandHandler : IRequestHandler<DeleteBasketItemCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DeleteBasketItemCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(DeleteBasketItemCommandRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result.Unauthorized("Sistemdə daxil edilməmisiniz. Zəhmət olmasa, daxil olun.");

        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.BasketItem, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.BasketItem, Guid>();

        var basketItem = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (basketItem == null)
            return Result.NotFound("Səbət elementi tapılmadı.");

        var basket = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Basket, Guid>()
            .GetByIdAsync(basketItem.BasketId, tracking: false, ct: ct);

        if (basket == null)
            return Result.NotFound("Səbət tapılmadı.");

        if (basket.UserId != userId && !_currentUserService.IsAdmin)
            return Result.Forbidden("Bu elementi silmək üçün icazəniz yoxdur.");

        writeRepo.Remove(basketItem);

        var result = await _unitOfWork.SaveAsync(ct);

        return result > 0
            ? Result.Success()
            : Result.Failure("Element səbətdən silinə bilmədi.");
    }
}
