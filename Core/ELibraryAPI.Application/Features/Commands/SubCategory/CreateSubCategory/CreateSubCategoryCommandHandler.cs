using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.Shared.Events;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.SubCategory.CreateSubCategory;

public sealed class CreateSubCategoryCommandHandler : IRequestHandler<CreateSubCategoryCommandRequest, Result<CreateSubCategoryCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator; 

    public CreateSubCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<Result<CreateSubCategoryCommandResponse>> Handle(CreateSubCategoryCommandRequest request, CancellationToken ct)
    {
        var subCategoryReadRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.SubCategory, Guid>();
        var subCategoryWriteRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.SubCategory, Guid>();
        var categoryReadRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Category, Guid>();

        // 1. Əsas kateqoriyanın varlığının yoxlanılması
        var categoryExists = await categoryReadRepository.ExistsAsync(x => x.Id == request.CategoryId, tracking: false, ct: ct);
        if (!categoryExists)
        {
            return Result<CreateSubCategoryCommandResponse>.Failure("Əsas kateqoriya tapılmadı.");
        }

        var normalizedName = request.Name.Trim();
        var isNameExists = await subCategoryReadRepository.ExistsAsync(
            x => x.Name.ToLower() == normalizedName.ToLower() && x.CategoryId == request.CategoryId,
            tracking: false,
            ct: ct);

        if (isNameExists)
        {
            return Result<CreateSubCategoryCommandResponse>.Failure("Bu kateqoriyada eyni adlı alt kateqoriya artıq mövcuddur.");
        }

        // 3. Mapping və Yaradılma
        var subCategory = _mapper.Map<Domain.Entities.Concrete.SubCategory>(request);
        subCategory.Name = normalizedName;

        await subCategoryWriteRepository.AddAsync(subCategory, ct);
        await _unitOfWork.SaveAsync(ct);

        await _mediator.Publish(new EntityChangedEvent("subcategory", subCategory.Id), ct);

        return Result<CreateSubCategoryCommandResponse>.Success(
            new CreateSubCategoryCommandResponse(subCategory.Id),
            "Alt kateqoriya uğurla yaradıldı.");
    }
}