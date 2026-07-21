using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Category.GetByIdCategory;

public sealed class GetByIdCategoryQueryHandler : IRequestHandler<GetByIdCategoryQueryRequest, Result<GetByIdCategoryQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdCategoryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetByIdCategoryQueryResponse>> Handle(GetByIdCategoryQueryRequest request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Category, Guid>()
            .GetAll(tracking: false) 
            .Where(c => c.Id == request.Id)
            .Select(c => new CategoryDetailDto(
                c.Id,
                c.Name,
                c.SubCategories.Select(sc => new SubCategoryItemDto(
                    sc.Id,
                    sc.Name,
                    sc.Products.Count 
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (category == null)
            return Result<GetByIdCategoryQueryResponse>.Failure("Category not found");

        return Result<GetByIdCategoryQueryResponse>.Success(new GetByIdCategoryQueryResponse(category));
    }
}