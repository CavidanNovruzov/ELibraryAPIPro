using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.SubCategory.UpdateSubCategory;

public sealed class UpdateSubCategoryCommandHandler : IRequestHandler<UpdateSubCategoryCommandRequest, Result<UpdateSubCategoryCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSubCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdateSubCategoryCommandResponse>> Handle(UpdateSubCategoryCommandRequest request, CancellationToken ct)
    {
        var subCategoryReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.SubCategory, Guid>();
        var categoryReadRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Category, Guid>();

        var subCategory = await subCategoryReadRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (subCategory == null)
            return Result<UpdateSubCategoryCommandResponse>.Failure("Sub-category not found.");

        var categoryExists = await categoryReadRepo.ExistsAsync(x => x.Id == request.CategoryId, false, ct);
        if (!categoryExists)
            return Result<UpdateSubCategoryCommandResponse>.Failure("Parent category not found.");

        var normalizedName = request.Name.Trim();
        if (subCategory.Name.ToLower() != normalizedName.ToLower() || subCategory.CategoryId != request.CategoryId)
        {
            var isNameExists = await subCategoryReadRepo.ExistsAsync(
                x => x.Name.ToLower() == normalizedName.ToLower() && x.CategoryId == request.CategoryId && x.Id != request.Id,
                tracking: false,
                ct: ct);

            if (isNameExists)
                return Result<UpdateSubCategoryCommandResponse>.Failure("A sub-category with this name already exists in this category.");
        }

        _mapper.Map(request, subCategory);
        subCategory.Name = normalizedName;

        await _unitOfWork.SaveAsync(ct);

        return Result<UpdateSubCategoryCommandResponse>.Success(
            new UpdateSubCategoryCommandResponse(subCategory.Id),
            "Sub-category updated successfully.");
    }
}