namespace ELibraryAPI.Application.Features.Queries.Branch.GetByIdBranch;

public sealed record GetByIdBranchQueryResponse(
    BranchDetailDto Branch
);

public sealed record BranchDetailDto(
    Guid Id,
    string Name,
    string Location,
    string Phone,
    List<BranchWorkHoursDto> WorkHours,
    int TotalStock
);

public sealed record BranchWorkHoursDto(
    DayOfWeek Day,
    TimeSpan OpenTime,
    TimeSpan CloseTime
);
