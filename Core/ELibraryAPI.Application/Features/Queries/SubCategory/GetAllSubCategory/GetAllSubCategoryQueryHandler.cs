using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.SubCategory.GetAllSubCategory;

public sealed class GetAllSubCategoryQueryHandler : IRequestHandler<GetAllSubCategoryQueryRequest, Result<GetAllSubCategoryQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllSubCategoryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllSubCategoryQueryResponse>> Handle(GetAllSubCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.SubCategory, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var subCategories = await query
            .OrderBy(sc => sc.Name)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(sc => new SubCategoryListDto(
                sc.Id,
                sc.Name,
                sc.CategoryId,
                sc.Category.Name,
                sc.Products.Count
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllSubCategoryQueryResponse>.Success(
            new GetAllSubCategoryQueryResponse(subCategories, totalCount, request.Page, request.Size, totalPages));
    }
}