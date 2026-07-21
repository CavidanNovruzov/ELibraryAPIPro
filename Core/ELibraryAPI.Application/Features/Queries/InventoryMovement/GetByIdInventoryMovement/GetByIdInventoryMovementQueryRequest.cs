using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.InventoryMovement.GetByIdInventoryMovement;

public sealed record GetByIdInventoryMovementQueryRequest(Guid Id) : IRequest<Result<GetByIdInventoryMovementQueryResponse>>;
