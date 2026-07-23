using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Category.DeleteCategory;

public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result> Handle(DeleteCategoryCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Category, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.Category, Guid>();

        var category = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (category == null)
        {
            return Result.Failure("Kateqoriya tapılmadı və ya artıq silinib.");
        }

        category.IsDeleted = true;
        writeRepo.Update(category);

        await _unitOfWork.SaveAsync(ct);

        await _mediator.Publish(new EntityChangedEvent("category", request.Id), ct);

        return Result.Success("Kateqoriya arxivə köçürüldü.");
    }
}