using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Genre.CreateGenre;

public sealed record CreateGenreCommandRequest(
    string Name
) : IRequest<Result<CreateGenreCommandResponse>>;
