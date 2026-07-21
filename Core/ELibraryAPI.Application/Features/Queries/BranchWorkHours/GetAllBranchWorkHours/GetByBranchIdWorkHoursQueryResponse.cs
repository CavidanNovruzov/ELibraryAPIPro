namespace ELibraryAPI.Application.Features.Queries.BranchWorkHours.GetAllBranchWorkHours;

public sealed record GetByBranchIdWorkHoursQueryResponse(
    Guid BranchId,
    string BranchName,
    List<BranchWorkHourDto> WorkHours
);

public sealed record BranchWorkHourDto(
    Guid Id,
    DayOfWeek Day,
    TimeSpan OpenTime,
    TimeSpan CloseTime
);