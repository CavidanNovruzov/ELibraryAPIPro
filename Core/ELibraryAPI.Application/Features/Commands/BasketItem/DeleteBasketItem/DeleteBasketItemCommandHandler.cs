using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.BasketItem.DeleteBasketItem;

public sealed class DeleteBasketItemCommandHandler : IRequestHandler<DeleteBasketItemCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBasketItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteBasketItemCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.BasketItem, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.BasketItem, Guid>();

        var basketItem = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct); // libraff.az — [tracking: true istifadə edildi]

        if (basketItem == null)
            return Result.Failure("Səbət elementi tapılmadı.");

        writeRepo.Remove(basketItem);

        var result = await _unitOfWork.SaveAsync(ct);

        return result > 0
            ? Result.Success()
            : Result.Failure("Element səbətdən silinə bilmədi.");
    }
}