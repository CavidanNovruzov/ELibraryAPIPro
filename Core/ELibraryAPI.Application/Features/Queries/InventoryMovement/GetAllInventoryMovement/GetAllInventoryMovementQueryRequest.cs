using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.InventoryMovement.GetAllInventoryMovement;

public sealed record GetAllInventoryMovementQueryRequest(int Page = 1, int Size = 20) : IRequest<Result<GetAllInventoryMovementQueryResponse>>;