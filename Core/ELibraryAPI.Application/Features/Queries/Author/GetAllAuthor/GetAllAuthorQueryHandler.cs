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
        var page = request.Page < 1 ? 1 : request.Page;
        var size = request.Size < 1 ? 10 : request.Size;

        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Author, Guid>()
            .GetAll(tracking: false);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.Trim().ToLower();
            query = query.Where(a => a.FullName.ToLower().Contains(search));
        }

        var totalCount = await query.CountAsync(ct);

        var authors = await query
            .OrderBy(a => a.FullName)
            .Skip((page - 1) * size)
            .Take(size)
            .Select(a => new AuthorListDto(
                a.Id,
                a.FullName,
                a.Country,
                a.ProductAuthors.Count
            ))
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling((double)totalCount / size);

        return Result<GetAllAuthorQueryResponse>.Success(
            new GetAllAuthorQueryResponse(authors, totalCount, page, size, totalPages));
    }
}