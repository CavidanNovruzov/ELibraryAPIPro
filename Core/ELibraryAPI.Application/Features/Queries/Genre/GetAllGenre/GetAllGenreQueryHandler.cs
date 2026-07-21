using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELibraryAPI.Application.Features.Queries.Genre.GetAllGenre;

public sealed class GetAllGenreQueryHandler : IRequestHandler<GetAllGenreQueryRequest, Result<GetAllGenreQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllGenreQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllGenreQueryResponse>> Handle(GetAllGenreQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Genre, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var genres = await query
            .OrderBy(g => g.Name)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(g => new GenreListDto(
                g.Id,
                g.Name,
                g.ProductGenres.Count
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllGenreQueryResponse>.Success(
            new GetAllGenreQueryResponse(genres, totalCount, request.Page, request.Size, totalPages));
    }
}