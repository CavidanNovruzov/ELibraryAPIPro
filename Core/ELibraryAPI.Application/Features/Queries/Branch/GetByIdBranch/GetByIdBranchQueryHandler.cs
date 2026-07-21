using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Branch.GetByIdBranch;

public sealed class GetByIdBranchQueryHandler : IRequestHandler<GetByIdBranchQueryRequest, Result<GetByIdBranchQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdBranchQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetByIdBranchQueryResponse>> Handle(GetByIdBranchQueryRequest request, CancellationToken ct)
    {
        var currentTime = DateTime.Now.TimeOfDay;
        var currentDay = DateTime.Now.DayOfWeek;

        var branchDto = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Branch, Guid>()
            .GetAll(tracking: false)
            .Where(b => b.Id == request.Id)
            .Select(b => new BranchDetailDto(
                b.Id,
                b.Name,
                b.Location,
                b.Phone,
                b.WorkHours.Select(wh => new BranchWorkHoursDto(
                    wh.Day,
                    wh.OpenTime,
                    wh.CloseTime
                )).ToList(),
                b.Stocks.Sum(s => s.Quantity)
            ))
            .FirstOrDefaultAsync(ct);

        if (branchDto == null)
            return Result<GetByIdBranchQueryResponse>.Failure("Filial tapılmadı.");

        return Result<GetByIdBranchQueryResponse>.Success(new GetByIdBranchQueryResponse(branchDto));
    }
}
