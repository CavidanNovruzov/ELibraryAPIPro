using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Genre.UpdateGenre;

public sealed record UpdateGenreCommandRequest(
    Guid Id,
    string Name
) : IRequest<Result<UpdateGenreCommandResponse>>;
