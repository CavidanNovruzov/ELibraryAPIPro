using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.PromoCode.DeletePromoCode;

public sealed class DeletePromoCodeCommandHandler : IRequestHandler<DeletePromoCodeCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePromoCodeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletePromoCodeCommandRequest request, CancellationToken ct)
    {
        var readRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.PromoCode, Guid>();
        var writeRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.PromoCode, Guid>();

        var promoCode = await readRepository.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (promoCode == null) return Result.Failure("Promo code not found.");

        writeRepository.Remove(promoCode);

        await _unitOfWork.SaveAsync(ct);
        return Result.Success("Promo code deleted successfully.");
    }
}