using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Genre.GetByIdGenre;

public sealed class GetByIdGenreQueryHandler : IRequestHandler<GetByIdGenreQueryRequest, Result<GetByIdGenreQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdGenreQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetByIdGenreQueryResponse>> Handle(GetByIdGenreQueryRequest request, CancellationToken cancellationToken)
    {
        var genre = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Genre, Guid>()
            .GetAll(tracking: false)
            .Where(g => g.Id == request.Id)
            .Select(g => new GenreDetailDto(
                g.Id,
                g.Name,
                g.ProductGenres.Count
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (genre == null)
            return Result<GetByIdGenreQueryResponse>.Failure("Genre not found.");

        return Result<GetByIdGenreQueryResponse>.Success(new GetByIdGenreQueryResponse(genre));
    }
}