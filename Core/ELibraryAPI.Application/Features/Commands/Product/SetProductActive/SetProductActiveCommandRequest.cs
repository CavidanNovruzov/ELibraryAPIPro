using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Product.SetProductActive;

public sealed record SetProductActiveCommandRequest(
    Guid Id,
    bool IsActive
) : IRequest<Result>;

