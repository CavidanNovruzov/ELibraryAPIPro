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
        var response = await _unitOfWork.ReadRepository<Domain.Entities.Concrete.Author, Guid>()
            .GetAll(tracking: false)
            .Where(a => a.Id == request.Id)
            .Select(a => new GetAuthorByIdQueryResponse(
                a.Id,
                a.FullName,
                a.Biography,
                a.Country,
                a.ProductAuthors.Select(pa => new AuthorBookDto(
                    pa.Product.Id,
                    pa.Product.Title,
                    pa.Product.SalePrice,
                    pa.Product.Images.FirstOrDefault(i => i.IsMain).ImageUrl
                )).ToList()
            ))
            .FirstOrDefaultAsync(ct);

        if (response is null)
            return Result<GetAuthorByIdQueryResponse>.NotFound("Müəllif tapılmadı.");

        return Result<GetAuthorByIdQueryResponse>.Success(response);
    }
}