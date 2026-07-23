using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.UserSearchHistory.DeleteUserSearchHistory;

public sealed class DeleteUserSearchHistoryCommandHandler : IRequestHandler<DeleteUserSearchHistoryCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserSearchHistoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteUserSearchHistoryCommandRequest request, CancellationToken ct)
    {
        var readRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.UserSearchHistory, Guid>();
        var writeRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.UserSearchHistory, Guid>();

        var historyRecord = await readRepository.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (historyRecord == null)
        {
            return Result.Failure("Axtarış tarixçəsi qeydi tapılmadı.", ErrorType.NotFound);
        }

        if (historyRecord.UserId != request.UserId)
        {
            return Result.Failure("Bu qeydi silmək icazəniz yoxdur.", ErrorType.Forbidden);
        }

        writeRepository.Remove(historyRecord);

        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Axtarış tarixçəsi qeydi silindi.");
    }
}