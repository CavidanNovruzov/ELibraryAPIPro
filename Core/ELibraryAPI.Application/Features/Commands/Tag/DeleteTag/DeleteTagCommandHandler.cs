using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Tag.DeleteTag;


public sealed class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTagCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteTagCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Tag, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Tag, Guid>();

        var tag = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (tag == null)
            return Result.Failure("Tag not found.");

        writeRepo.Remove(tag);
        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Tag deleted successfully.");
    }
}