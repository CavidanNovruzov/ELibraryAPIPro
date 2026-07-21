using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Author.CreateAuthor;

public sealed record CreateAuthorCommandRequest(
    string Biography,
    string Country,
    string FullName,
    string ImagePath
) : IRequest<Result<CreateAuthorCommandResponse>>;
