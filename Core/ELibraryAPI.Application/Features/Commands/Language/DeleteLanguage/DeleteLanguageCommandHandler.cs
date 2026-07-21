using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Language.DeleteLanguage;

public sealed class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLanguageCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteLanguageCommandRequest request, CancellationToken ct)
    {
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Language, Guid>();

        var isRemoved = await writeRepo.RemoveAsync(request.Id, ct);

        if (!isRemoved)
            return Result.Failure("Language not found.");

        await _unitOfWork.SaveAsync(ct);
        return Result.Success("Language deleted successfully.");
    }
}