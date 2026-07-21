using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.CoverType.GetByIdCoverType;

public sealed class GetByIdCoverTypeQueryHandler : IRequestHandler<GetByIdCoverTypeQueryRequest, Result<GetByIdCoverTypeQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdCoverTypeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetByIdCoverTypeQueryResponse>> Handle(GetByIdCoverTypeQueryRequest request, CancellationToken cancellationToken)
    {
        var coverType = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.CoverType, Guid>()
            .GetAll(tracking: false)
            .Where(ct => ct.Id == request.Id)
            .Select(ct => new CoverTypeDetailDto(
                ct.Id,
                ct.Name,
                ct.Products.Count
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (coverType == null)
            return Result<GetByIdCoverTypeQueryResponse>.Failure("Cover type not found.");

       return Result<GetByIdCoverTypeQueryResponse>.Success(new GetByIdCoverTypeQueryResponse(coverType));
    }
}
