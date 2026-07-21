using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.SubCategory.CreateSubCategory;

public sealed class CreateSubCategoryCommandHandler : IRequestHandler<CreateSubCategoryCommandRequest, Result<CreateSubCategoryCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSubCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateSubCategoryCommandResponse>> Handle(CreateSubCategoryCommandRequest request, CancellationToken ct)
    {
        var subCategoryReadRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.SubCategory, Guid>();
        var subCategoryWriteRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.SubCategory, Guid>();
        var categoryReadRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Category, Guid>();

        var categoryExists = await categoryReadRepository.ExistsAsync(x => x.Id == request.CategoryId, false, ct);
        if (!categoryExists)
        {
            return Result<CreateSubCategoryCommandResponse>.Failure("Parent category not found.");
        }

        var normalizedName = request.Name.Trim();
        var isNameExists = await subCategoryReadRepository.ExistsAsync(
            x => x.Name.ToLower() == normalizedName.ToLower() && x.CategoryId == request.CategoryId,
            tracking: false,
            ct: ct);

        if (isNameExists)
        {
            return Result<CreateSubCategoryCommandResponse>.Failure("A sub-category with this name already exists in this category.");
        }

        var subCategory = _mapper.Map<Domain.Entities.Concrete.SubCategory>(request);
        subCategory.Name = normalizedName;

        await subCategoryWriteRepository.AddAsync(subCategory, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateSubCategoryCommandResponse>.Success(
            new CreateSubCategoryCommandResponse(subCategory.Id),
            "Sub-category created successfully.");
    }
}