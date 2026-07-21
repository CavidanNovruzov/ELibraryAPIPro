using ELibraryAPI.Application.UnitOfWork;
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Author.DeleteAuthor;

public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAuthorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteAuthorCommandRequest request, CancellationToken ct)
    {
        var writeRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Author, Guid>();

        var author = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Author, Guid>().GetByIdAsync(request.Id, tracking: true, ct); 
        if (author is null)
            return Result.Failure("Author not found or already deleted.");

        writeRepository.Remove(author);

        await _unitOfWork.SaveAsync(ct);

        return Result.Success();
    }
}