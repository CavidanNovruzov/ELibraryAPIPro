using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Author.GetAuthorsByAlphabet;

public sealed class GetAuthorsByAlphabetQueryHandler : IRequestHandler<GetAuthorsByAlphabetQueryRequest, Result<GetAuthorsByAlphabetQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAuthorsByAlphabetQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAuthorsByAlphabetQueryResponse>> Handle(GetAuthorsByAlphabetQueryRequest request, CancellationToken ct)
    {
        var letter = char.ToUpper(request.Letter).ToString();

        var authors = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Author, Guid>()
            .GetAll(false)
            .Include(a => a.ProductAuthors)
            .Where(a => a.FullName.StartsWith(letter))
            .OrderBy(a => a.FullName)
            .Select(a => new AuthorAlphabetDto(
                a.Id,
                a.FullName,
                a.ProductAuthors.Count
            ))
            .ToListAsync(ct);

        return Result<GetAuthorsByAlphabetQueryResponse>.Success(new GetAuthorsByAlphabetQueryResponse(authors));
    }
}