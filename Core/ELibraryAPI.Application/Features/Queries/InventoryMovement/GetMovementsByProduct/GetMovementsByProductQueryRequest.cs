using ELibraryAPI.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Queries.InventoryMovement.GetMovementsByProduct;

public sealed record GetMovementsByProductQueryRequest(Guid ProductId) : IRequest<Result<GetMovementsByProductQueryResponse>>;
