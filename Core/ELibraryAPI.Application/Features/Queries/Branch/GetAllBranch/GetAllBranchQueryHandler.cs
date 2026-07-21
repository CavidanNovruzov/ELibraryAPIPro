using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELibraryAPI.Application.Features.Queries.Branch.GetAllBranch;

public sealed class GetAllBranchQueryHandler : IRequestHandler<GetAllBranchQueryRequest, Result<GetAllBranchQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllBranchQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllBranchQueryResponse>> Handle(GetAllBranchQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Branch, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var branches = await query
            .OrderBy(b => b.Name)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(b => new BranchListDto(
                b.Id,
                b.Name,
                b.Location,
                b.Phone,
                b.WorkHours.Count
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllBranchQueryResponse>.Success(
            new GetAllBranchQueryResponse(branches, totalCount, request.Page, request.Size, totalPages));
    }
}