using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Author.DeleteAuthor;

public sealed record DeleteAuthorCommandRequest(Guid Id) : IRequest<Result>;
