using ELibraryAPI.Application.Abstractions.Services;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Domain.Enums;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.UserSearchHistory.DeleteUserSearchHistory;

public sealed class DeleteUserSearchHistoryCommandHandler : IRequestHandler<DeleteUserSearchHistoryCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public DeleteUserSearchHistoryCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(DeleteUserSearchHistoryCommandRequest request, CancellationToken ct)
    {
        var userId = _currentUserService.UserGuid;
        if (userId == Guid.Empty)
            return Result.Unauthorized("Sistemdə daxil edilməmisiniz. Zəhmət olmasa, daxil olun.");

        var readRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.UserSearchHistory, Guid>();
        var writeRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.UserSearchHistory, Guid>();

        var historyRecord = await readRepository.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (historyRecord == null)
        {
            return Result.NotFound("Axtarış tarixçəsi qeydi tapılmadı.");
        }

        if (historyRecord.UserId != userId && !_currentUserService.IsAdmin)
        {
            return Result.Forbidden("Bu qeydi silmək icazəniz yoxdur.");
        }

        writeRepository.Remove(historyRecord);

        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Axtarış tarixçəsi qeydi silindi.");
    }
}
