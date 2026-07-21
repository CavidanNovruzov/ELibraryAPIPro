using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Genre.DeleteGenre;

public sealed class DeleteGenreCommandHandler : IRequestHandler<DeleteGenreCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGenreCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteGenreCommandRequest request, CancellationToken ct)
    {
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Genre, Guid>();

        var isRemoved = await writeRepo.RemoveAsync(request.Id, ct);

        if (!isRemoved)
        {
            return Result.Failure("Genre not found or already deleted.");
        }

        var result = await _unitOfWork.SaveAsync(ct);

        return result > 0
            ? Result.Success("Genre deleted successfully.")
            : Result.Failure("An error occurred while deleting the genre.");
    }
}