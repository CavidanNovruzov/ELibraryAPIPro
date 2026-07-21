using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ELibraryAPI.Application.Features.Queries.Publisher.GetAllPublisher;

public sealed class GetAllPublisherQueryHandler : IRequestHandler<GetAllPublisherQueryRequest, Result<GetAllPublisherQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllPublisherQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllPublisherQueryResponse>> Handle(GetAllPublisherQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Publisher, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var publishers = await query
            .OrderBy(p => p.Name)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(p => new PublisherListDto(
                p.Id,
                p.Name,
                p.Products.Count
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllPublisherQueryResponse>.Success(
            new GetAllPublisherQueryResponse(publishers, totalCount, request.Page, request.Size, totalPages));
    }
}