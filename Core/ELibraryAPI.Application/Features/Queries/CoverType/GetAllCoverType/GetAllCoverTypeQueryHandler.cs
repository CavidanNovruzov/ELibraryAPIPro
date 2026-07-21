using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELibraryAPI.Application.Features.Queries.CoverType.GetAllCoverType;

public sealed class GetAllCoverTypeQueryHandler : IRequestHandler<GetAllCoverTypeQueryRequest, Result<GetAllCoverTypeQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCoverTypeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllCoverTypeQueryResponse>> Handle(GetAllCoverTypeQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.CoverType, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var coverTypes = await query
            .OrderBy(ct => ct.Name)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(ct => new CoverTypeListDto(
                ct.Id,
                ct.Name,
                ct.Products.Count
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllCoverTypeQueryResponse>.Success(
            new GetAllCoverTypeQueryResponse(coverTypes, totalCount, request.Page, request.Size, totalPages));
    }
}