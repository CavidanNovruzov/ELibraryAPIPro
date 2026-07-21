using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.BranchWorkHours.UpdateBranchWorkHours;

public sealed record UpdateBranchWorkHoursCommandRequest(
    Guid Id,
    Guid BranchId,
    TimeSpan CloseTime,
    DayOfWeek Day,
    TimeSpan OpenTime
) : IRequest<Result<UpdateBranchWorkHoursCommandResponse>>;
