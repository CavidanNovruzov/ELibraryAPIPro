using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Author.GetAuthorById;

public sealed class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQueryRequest, Result<GetAuthorByIdQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAuthorByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAuthorByIdQueryResponse>> Handle(GetAuthorByIdQueryRequest request, CancellationToken ct)
    {
        var author = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Author, Guid>()
            .GetAll(false)
            .Include(a => a.ProductAuthors)
                .ThenInclude(pa => pa.Product)
                    .ThenInclude(p => p.Images)
            .FirstOrDefaultAsync(a => a.Id == request.Id, ct);

        if (author == null)
            return Result<GetAuthorByIdQueryResponse>.Failure("Author not found.");

        var response = new GetAuthorByIdQueryResponse(
            author.Id,
            author.FullName,
            author.Biography,
            author.Country,
            author.ProductAuthors.Select(pa => new AuthorBookDto(
                pa.Product.Id,
                pa.Product.Title,
                pa.Product.SalePrice,
                pa.Product.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl
            )).ToList()
        );

        return Result<GetAuthorByIdQueryResponse>.Success(response);
    }
}