using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.SubCategory.GetByIdSubCategory;

public sealed class GetByIdSubCategoryQueryHandler : IRequestHandler<GetByIdSubCategoryQueryRequest, Result<GetByIdSubCategoryQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetByIdSubCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetByIdSubCategoryQueryResponse>> Handle(GetByIdSubCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        var subCategory = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.SubCategory, Guid>()
            .GetAll(tracking: false)
            .Where(sc => sc.Id == request.Id)
            .Select(sc => new SubCategoryDetailDto(
                sc.Id,
                sc.Name,
                sc.CategoryId,
                sc.Category.Name,
                sc.Products.Count
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (subCategory == null)
            return Result<GetByIdSubCategoryQueryResponse>.Failure("Sub-category was not found.");

        return Result<GetByIdSubCategoryQueryResponse>.Success(new GetByIdSubCategoryQueryResponse(subCategory));
    }
}