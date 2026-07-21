using System;
using System.Collections.Generic;

namespace ELibraryAPI.Application.Features.Queries.Branch.GetAllBranch;

public sealed record GetAllBranchQueryResponse(
    List<BranchListDto> Branches,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record BranchListDto(Guid Id, string Name, string Location, string Phone, int WorkHoursCount);