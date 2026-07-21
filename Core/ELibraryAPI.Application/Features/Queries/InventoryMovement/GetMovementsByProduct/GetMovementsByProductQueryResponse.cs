using ELibraryAPI.Application.Features.Queries.InventoryMovement.GetAllInventoryMovement;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Queries.InventoryMovement.GetMovementsByProduct;

public sealed record GetMovementsByProductQueryResponse(List<InventoryMovementListDto> Movements);
