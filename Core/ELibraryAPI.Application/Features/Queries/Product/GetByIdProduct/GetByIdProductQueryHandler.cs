using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Product.GetByIdProduct;

public sealed class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, Result<GetByIdProductQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdProductQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetByIdProductQueryResponse>> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
    {
        var productEntity = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Product, Guid>()
            .GetAll(tracking: false)
            .Include(p => p.Publisher)
            .Include(p => p.Language)
            .Include(p => p.CoverType)
            .Include(p => p.SubCategory)
                .ThenInclude(sc => sc.Category)
            .Include(p => p.Stocks)
            .Include(p => p.Images)
            .Include(p => p.ProductAuthors)
                .ThenInclude(pa => pa.Author)
            .Include(p => p.ProductGenres)
                .ThenInclude(pg => pg.Genre)
            .Include(p => p.ProductTags)
                .ThenInclude(pt => pt.Tag)
            .Include(p => p.Reviews)
                .ThenInclude(r => r.User)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (productEntity == null)
            return Result<GetByIdProductQueryResponse>.Failure("Product not found");

        var dto = new ProductDetailDto(
            productEntity.Id,
            productEntity.Title,
            productEntity.Description,
            productEntity.ISBN,
            productEntity.PageCount,
            productEntity.PublicationYear,
            productEntity.SalePrice,
            productEntity.DiscountPrice,
            productEntity.Stocks.Sum(s => s.Quantity),
            productEntity.Publisher?.Name ?? "",
            productEntity.Language?.Name ?? "",
            productEntity.CoverType?.Name ?? "",
            productEntity.SubCategory?.Category?.Name ?? "",
            productEntity.SubCategory?.Name ?? "",
            productEntity.Images.Select(i => new ProductImageDto(i.Id, i.ImageUrl, i.IsMain)).ToList(),
            
            productEntity.ProductAuthors.Select(pa => pa.Author?.FullName ?? "").Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
            productEntity.ProductGenres.Select(pg => pg.Genre?.Name ?? "").Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
            productEntity.ProductTags.Select(pt => pt.Tag?.Name ?? "").Where(x => !string.IsNullOrWhiteSpace(x)).ToList(),
            productEntity.Reviews.Select(r => new ReviewItemDto(r.Id, r.User?.Email ?? "", r.Comment, r.Rating, r.CreatedDate)).ToList(),
            productEntity.Reviews.Count,
            productEntity.Reviews.Any() ? productEntity.Reviews.Average(r => r.Rating) : 0
        );

        return Result<GetByIdProductQueryResponse>.Success(new GetByIdProductQueryResponse(dto));
    }
}