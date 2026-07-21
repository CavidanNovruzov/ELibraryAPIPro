using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Commands.SubCategory.MergeSubCategories;

public sealed class MergeSubCategoriesCommandHandler : IRequestHandler<MergeSubCategoriesCommandRequest, Result<MergeSubCategoriesCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public MergeSubCategoriesCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<MergeSubCategoriesCommandResponse>> Handle(MergeSubCategoriesCommandRequest request, CancellationToken ct)
    {
        var subCategoryReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.SubCategory, Guid>();
        var subCategoryWriteRepo = _unitOfWork.WriteRepository<Domain.Entities.Concrete.SubCategory, Guid>();
        var productReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Product, Guid>();

        var sourceSubCategory = await subCategoryReadRepo.GetByIdAsync(request.SourceSubCategoryId, tracking: true, ct: ct);
        var targetSubCategory = await subCategoryReadRepo.GetByIdAsync(request.TargetSubCategoryId, tracking: true, ct: ct);

        if (sourceSubCategory == null || targetSubCategory == null)
            return Result<MergeSubCategoriesCommandResponse>.Failure("One or both sub-categories not found.");


        var productsToMove = await productReadRepo.GetWhere(x => x.SubCategoryId == request.SourceSubCategoryId, tracking: true)
                                                  .ToListAsync(ct);

        await using var transaction = await _unitOfWork.BeginTransactionAsync(ct);
        try
        {
            foreach (var product in productsToMove)
                product.SubCategoryId = request.TargetSubCategoryId;

            subCategoryWriteRepo.Remove(sourceSubCategory);

            await _unitOfWork.SaveAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }

        return Result<MergeSubCategoriesCommandResponse>.Success(
            new MergeSubCategoriesCommandResponse(targetSubCategory.Id),
            "Sub-categories merged successfully.");
    }
}