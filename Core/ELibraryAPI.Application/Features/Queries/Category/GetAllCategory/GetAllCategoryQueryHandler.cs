using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Application.Features.Queries.Category.GetAllCategory;

public sealed class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQueryRequest, Result<GetAllCategoryQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCategoryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllCategoryQueryResponse>> Handle(GetAllCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Category, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var categories = await query
            .OrderBy(c => c.Name)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(c => new CategoryListDto(
                c.Id,
                c.Name,
                c.SubCategories.Count,
                c.SubCategories.SelectMany(sc => sc.Products).Count()
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllCategoryQueryResponse>.Success(
            new GetAllCategoryQueryResponse(categories, totalCount, request.Page, request.Size, totalPages));
    }
}