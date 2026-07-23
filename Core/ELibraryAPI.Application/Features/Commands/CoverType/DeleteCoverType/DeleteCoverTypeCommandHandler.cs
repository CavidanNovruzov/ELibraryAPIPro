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
            return Result.Failure("Üz qabığı növü tapılmadı və ya artıq silinib.");
        }

        var result = await _unitOfWork.SaveAsync(ct);

        return result > 0
            ? Result.Success("Üz qabığı növü uğurla silindi.")
            : Result.Failure("Üz qabığı növü silinərkən xəta baş verdi.");
    }
}