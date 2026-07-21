using ELibraryAPI.Application.Features.Commands.Product.UpdateProduct;
using ELibraryAPI.Application.Responses;
using MediatR;

public sealed record UpdateProductCommandRequest(
    Guid Id,
    string Title,
    string Description,
    string ISBN,
    int PageCount,
    decimal SalePrice,
    decimal? DiscountPrice,
    int PublicationYear,
    Guid PublisherId,
    Guid LanguageId,
    Guid CoverTypeId,
    Guid SubCategoryId,
    List<Guid> AuthorIds,
    List<Guid> GenreIds,
    List<Guid> TagIds,
    List<UpdateProductImageDto> Images
) : IRequest<Result<UpdateProductCommandResponse>>;

public record UpdateProductImageDto(string ImageUrl, bool IsMain);
