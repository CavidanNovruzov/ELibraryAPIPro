using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Tag.DeleteTag;

public sealed record DeleteTagCommandRequest(Guid Id) : IRequest<Result>;
