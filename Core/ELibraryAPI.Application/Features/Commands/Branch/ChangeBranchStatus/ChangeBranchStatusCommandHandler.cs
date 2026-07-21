using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Branch.ChangeBranchStatus;

public sealed class ChangeBranchStatusCommandHandler : IRequestHandler<ChangeBranchStatusCommandRequest, Result<ChangeBranchStatusCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangeBranchStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ChangeBranchStatusCommandResponse>> Handle(ChangeBranchStatusCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Branch, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Branch, Guid>();

        var branch = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (branch == null)
            return Result<ChangeBranchStatusCommandResponse>.Failure("Branch not found.");

        // Branch entity-də IsActive və ya Status sahəsi olduğunu fərz edirik
        // branch.IsActive = request.IsActive; 

        writeRepo.Update(branch);
        await _unitOfWork.SaveAsync(ct);

        return Result<ChangeBranchStatusCommandResponse>.Success(
            new ChangeBranchStatusCommandResponse(branch.Id, request.IsActive),
            "Branch status changed successfully.");
    }
}