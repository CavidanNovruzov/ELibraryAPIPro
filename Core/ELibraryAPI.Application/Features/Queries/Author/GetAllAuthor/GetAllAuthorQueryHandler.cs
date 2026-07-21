using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Application.Features.Queries.Author.GetAllAuthor;

public sealed class GetAllAuthorQueryHandler : IRequestHandler<GetAllAuthorQueryRequest, Result<GetAllAuthorQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllAuthorQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllAuthorQueryResponse>> Handle(GetAllAuthorQueryRequest request, CancellationToken ct)
    {
        var query = _unitOfWork.ReadRepository<Domain.Entities.Concrete.Author, Guid>().GetAll(false);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.Trim();
            query = query.Where(a => a.FullName.Contains(search));
        }

        var totalCount = await query.CountAsync(ct);

        var authors = await query
            .OrderBy(a => a.FullName)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(a => new AuthorListDto(
                a.Id,
                a.FullName,
                a.Country,
                a.ProductAuthors.Count
            ))
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllAuthorQueryResponse>.Success(
            new GetAllAuthorQueryResponse(authors, totalCount, request.Page, request.Size, totalPages));
    }
}