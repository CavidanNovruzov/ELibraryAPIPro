using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.UserAddress.DeleteUserAddress;


public sealed class DeleteUserAddressCommandHandler : IRequestHandler<DeleteUserAddressCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserAddressCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteUserAddressCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.UserAddress, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.UserAddress, Guid>();

        var address = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (address == null) return Result.Failure("Address not found.");

        writeRepo.Remove(address);
        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Address deleted successfully.");
    }
}