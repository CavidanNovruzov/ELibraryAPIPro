using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.CoverType.DeleteCoverType;

public sealed class DeleteCoverTypeCommandHandler : IRequestHandler<DeleteCoverTypeCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCoverTypeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCoverTypeCommandRequest request, CancellationToken ct)
    {

        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.CoverType, Guid>();

        var isRemoved = await writeRepo.RemoveAsync(request.Id, ct);

        if (!isRemoved)
        {
            return Result.Failure("Cover type not found or already deleted.");
        }

        var result = await _unitOfWork.SaveAsync(ct);

        return result > 0
            ? Result.Success("Cover type deleted successfully.")
            : Result.Failure("An error occurred while deleting the cover type.");
    }
}