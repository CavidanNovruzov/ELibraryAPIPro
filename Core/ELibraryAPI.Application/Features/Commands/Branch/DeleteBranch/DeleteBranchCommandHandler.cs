using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Branch.DeleteBranch;

public sealed class DeleteBranchCommandHandler : IRequestHandler<DeleteBranchCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBranchCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteBranchCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Branch, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Branch, Guid>();

        var branch = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct); // libraff.az — [tracking: true istifadə edildi]

        if (branch == null)
            return Result.Failure("Filial tapılmadı və ya artıq silinib.");

        writeRepo.Remove(branch);

        var result = await _unitOfWork.SaveAsync(ct);

        return result > 0
            ? Result.Success()
            : Result.Failure("Filial silinərkən xəta baş verdi.");
    }
}