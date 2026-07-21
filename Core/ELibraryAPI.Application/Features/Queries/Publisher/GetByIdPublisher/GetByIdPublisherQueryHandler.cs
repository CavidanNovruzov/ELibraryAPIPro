using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Publisher.GetByIdPublisher;

public sealed class GetByIdPublisherQueryHandler : IRequestHandler<GetByIdPublisherQueryRequest, Result<GetByIdPublisherQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdPublisherQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetByIdPublisherQueryResponse>> Handle(GetByIdPublisherQueryRequest request, CancellationToken cancellationToken)
    {
        var publisher = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Publisher, Guid>()
            .GetAll(tracking: false) 
            .Where(p => p.Id == request.Id)
            .Select(p => new PublisherDetailDto(
                p.Id,
                p.Name,
                p.Description ?? string.Empty,
                p.Products.Count
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (publisher == null)
            return Result<GetByIdPublisherQueryResponse>.Failure("Publisher not found");

        return Result<GetByIdPublisherQueryResponse>.Success(new GetByIdPublisherQueryResponse(publisher));
    }
}