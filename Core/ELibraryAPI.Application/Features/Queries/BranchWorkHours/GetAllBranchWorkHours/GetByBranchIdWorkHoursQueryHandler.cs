using ELibraryAPI.Application.Features.Queries.BranchWorkHours.GetAllBranchWorkHours;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELibraryAPI.Application.Features.Queries.BranchWorkHours.GetByBranchId;

public sealed class GetByBranchIdWorkHoursQueryHandler : IRequestHandler<GetByBranchIdWorkHoursQueryRequest, Result<GetByBranchIdWorkHoursQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByBranchIdWorkHoursQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetByBranchIdWorkHoursQueryResponse>> Handle(GetByBranchIdWorkHoursQueryRequest request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Branch, Guid>()
            .GetAll(tracking: false)
            .Where(b => b.Id == request.BranchId)
            .Select(b => new GetByBranchIdWorkHoursQueryResponse(
                b.Id,
                b.Name,
                b.WorkHours
                    .OrderBy(wh => wh.Day)
                    .Select(wh => new BranchWorkHourDto(
                        wh.Id,
                        wh.Day,
                        wh.OpenTime,
                        wh.CloseTime
                    )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (result == null)
            return Result<GetByBranchIdWorkHoursQueryResponse>.Failure("Seçilmiş filial tapılmadı.");

        return Result<GetByBranchIdWorkHoursQueryResponse>.Success(result);
    }
}