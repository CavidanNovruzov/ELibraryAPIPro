using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ELibraryAPI.Application.Features.Queries.Language.GetAllLanguage;

public sealed class GetAllLanguageQueryHandler : IRequestHandler<GetAllLanguageQueryRequest, Result<GetAllLanguageQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllLanguageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetAllLanguageQueryResponse>> Handle(GetAllLanguageQueryRequest request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Language, Guid>()
            .GetAll(tracking: false);

        var totalCount = await query.CountAsync(cancellationToken);

        var languages = await query
            .OrderBy(l => l.Name)
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .Select(l => new LanguageListDto(
                l.Id,
                l.Name,
                l.Code,
                l.Products.Count
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / request.Size);

        return Result<GetAllLanguageQueryResponse>.Success(
            new GetAllLanguageQueryResponse(languages, totalCount, request.Page, request.Size, totalPages));
    }
}