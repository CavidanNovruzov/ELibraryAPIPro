using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Application.Features.Queries.Product.GetAllProduct;

public sealed class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, Result<GetAllProductQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllProductQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllProductQueryResponse>> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Product, Guid>()
            .GetAll(tracking: false);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(p => p.Title.Contains(request.Search) || p.ISBN.Contains(request.Search));

        if (request.CategoryId.HasValue)
            query = query.Where(p => p.SubCategory.CategoryId == request.CategoryId);

        if (request.SubCategoryId.HasValue)
            query = query.Where(p => p.SubCategoryId == request.SubCategoryId);

        if (request.AuthorId.HasValue)
            query = query.Where(p => p.ProductAuthors.Any(pa => pa.AuthorId == request.AuthorId));

        if (request.GenreId.HasValue)
            query = query.Where(p => p.ProductGenres.Any(pg => pg.GenreId == request.GenreId));

        if (request.MinPrice.HasValue)
            query = query.Where(p => (p.DiscountPrice ?? p.SalePrice) >= request.MinPrice);

        if (request.MaxPrice.HasValue)
            query = query.Where(p => (p.DiscountPrice ?? p.SalePrice) <= request.MaxPrice);

        if (request.IsInDiscount == true)
            query = query.Where(p => p.DiscountPrice.HasValue && p.DiscountPrice < p.SalePrice);

        query = request.SortBy switch
        {
            "PriceAsc" => query.OrderBy(p => p.DiscountPrice ?? p.SalePrice),
            "PriceDesc" => query.OrderByDescending(p => p.DiscountPrice ?? p.SalePrice),
            "TopRated" => query.OrderByDescending(p => p.Reviews.Average(r => (double?)r.Rating) ?? 0),
            _ => query.OrderByDescending(p => p.CreatedDate)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var products = await query
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(p => new ProductListDto(
                p.Id,
                p.Title,
                p.ISBN,
                p.SalePrice,
                p.DiscountPrice,
                p.PageCount,
                p.Publisher.Name,
                p.Language.Name,
                p.CoverType.Name,
                p.SubCategory.Category.Name,
                p.ProductAuthors.Select(pa => pa.Author.FullName).ToList(),
                p.ProductGenres.Select(pg => pg.Genre.Name).ToList(),
                p.Reviews.Count,
                p.Reviews.Average(r => (double?)r.Rating) ?? 0,
                p.Images.Where(i => i.IsMain).Select(i => i.ImageUrl).FirstOrDefault() ?? ""
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllProductQueryResponse>.Success(
            new GetAllProductQueryResponse(
                products,
                totalCount,
                request.Page,
                request.Size,
                totalPages
            ));
    }
}