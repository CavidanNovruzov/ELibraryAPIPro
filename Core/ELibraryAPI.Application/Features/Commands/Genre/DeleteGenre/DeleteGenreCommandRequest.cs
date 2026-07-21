using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Genre.DeleteGenre;

public sealed record DeleteGenreCommandRequest(Guid Id) : IRequest<Result>;
