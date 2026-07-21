using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.SubCategory.DeleteSubCategory;

public sealed class DeleteSubCategoryCommandHandler : IRequestHandler<DeleteSubCategoryCommandRequest, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSubCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteSubCategoryCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.SubCategory, Guid>();
        var writeRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.SubCategory, Guid>();

        var subCategory = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (subCategory == null)
            return Result.Failure("Sub-category not found.");

        writeRepo.Remove(subCategory);
        await _unitOfWork.SaveAsync(ct);

        return Result.Success("Sub-category deleted successfully.");
    }
}