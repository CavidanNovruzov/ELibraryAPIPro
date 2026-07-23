using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Author.DeleteAuthor;

public sealed class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public DeleteAuthorCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result> Handle(DeleteAuthorCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Author, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Author, Guid>();

        var author = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);
        if (author is null)
            return Result.Failure("Müəllif tapılmadı və ya artıq silinib.");

        writeRepo.Remove(author);

        var saveResult = await _unitOfWork.SaveAsync(ct);

        if (saveResult > 0)
        {
            await _mediator.Publish(new EntityChangedEvent("author", request.Id), ct);

            return Result.Success("Müəllif uğurla silindi.");
        }

        return Result.Failure("Müəllif silinərkən xəta baş verdi.");
    }
}