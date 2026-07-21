using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.BranchWorkHours.DeleteBranchWorkHours;

public sealed class DeleteBranchWorkHoursCommandHandler : IRequestHandler<DeleteBranchWorkHoursCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBranchWorkHoursCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteBranchWorkHoursCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.BranchWorkHours, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.BranchWorkHours, Guid>();

        var workHours = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (workHours == null)
        {
            return Result.Failure("Work hours not found.");
        }

        writeRepo.Remove(workHours);

        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Branch work hours deleted successfully.");
    }
}