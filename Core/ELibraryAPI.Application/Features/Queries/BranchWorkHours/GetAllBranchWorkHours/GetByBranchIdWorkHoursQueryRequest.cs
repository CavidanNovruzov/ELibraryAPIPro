using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.BranchWorkHours.GetAllBranchWorkHours;

public sealed record GetByBranchIdWorkHoursQueryRequest(Guid BranchId) : IRequest<Result<GetByBranchIdWorkHoursQueryResponse>>;