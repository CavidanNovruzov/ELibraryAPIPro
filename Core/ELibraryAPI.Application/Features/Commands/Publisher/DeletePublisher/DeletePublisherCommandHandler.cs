using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Publisher.DeletePublisher;

public sealed class DeletePublisherCommandHandler : IRequestHandler<DeletePublisherCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePublisherCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletePublisherCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Publisher, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Publisher, Guid>();

        var publisher = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (publisher is null)
        {
            return Result.Failure("Publisher not found.");
        }

        writeRepo.Remove(publisher);

        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Publisher has been successfully deleted.");
    }
}