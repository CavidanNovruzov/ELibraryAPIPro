using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.CoverType.DeleteCoverType;

public sealed record DeleteCoverTypeCommandRequest(Guid Id) : IRequest<Result>;
