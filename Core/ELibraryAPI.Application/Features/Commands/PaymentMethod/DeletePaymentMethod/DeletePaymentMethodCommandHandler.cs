using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.PaymentMethod.DeletePaymentMethod;

public sealed class DeletePaymentMethodCommandHandler : IRequestHandler<DeletePaymentMethodCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePaymentMethodCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletePaymentMethodCommandRequest request, CancellationToken ct)
    {
        var readRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.PaymentMethod, Guid>();
        var writeRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.PaymentMethod, Guid>();

        var paymentMethod = await readRepository.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (paymentMethod == null)
        {
            return Result.Failure("Payment method not found.");
        }

        writeRepository.Remove(paymentMethod);

        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Payment method deleted successfully.");
    }
}