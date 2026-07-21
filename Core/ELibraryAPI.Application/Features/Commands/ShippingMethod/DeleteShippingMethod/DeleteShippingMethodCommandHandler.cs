using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.ShippingMethod.DeleteShippingMethod;

public sealed class DeleteShippingMethodCommandHandler : IRequestHandler<DeleteShippingMethodCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteShippingMethodCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteShippingMethodCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.ShippingMethod, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.ShippingMethod, Guid>();

        var shippingMethod = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (shippingMethod == null)
            return Result.Failure("Shipping method not found.");

        writeRepo.Remove(shippingMethod);
        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Shipping method deleted successfully.");
    }
}