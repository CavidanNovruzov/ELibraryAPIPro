using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.BranchWorkHours.CreateBranchWorkHours;

public sealed record CreateBranchWorkHoursCommandRequest(
    Guid BranchId,
    TimeSpan CloseTime,
    DayOfWeek Day,
    TimeSpan OpenTime
) : IRequest<Result<CreateBranchWorkHoursCommandResponse>>;
