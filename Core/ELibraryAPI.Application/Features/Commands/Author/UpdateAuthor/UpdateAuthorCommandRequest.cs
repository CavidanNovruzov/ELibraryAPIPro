using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Author.UpdateAuthor;

public sealed record UpdateAuthorCommandRequest(
    Guid Id,
    string Biography,
    string Country,
    string FullName,
    string ImagePath
) : IRequest<Result<UpdateAuthorCommandResponse>>;
