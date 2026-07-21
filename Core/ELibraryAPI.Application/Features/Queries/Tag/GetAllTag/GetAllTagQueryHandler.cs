using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Tag.GetAllTag;

public sealed class GetAllTagQueryHandler : IRequestHandler<GetAllTagQueryRequest, Result<GetAllTagQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTagQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllTagQueryResponse>> Handle(GetAllTagQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Tag, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var tags = await query
            .OrderBy(t => t.Name)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(t => new TagListDto(
                t.Id,
                t.Name
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllTagQueryResponse>.Success(
            new GetAllTagQueryResponse(tags, totalCount, request.Page, request.Size, totalPages));
    }
}