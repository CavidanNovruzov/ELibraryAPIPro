using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ELibraryAPI.Application.Features.Queries.Language.GetByIdLanguage;

public sealed class GetByIdLanguageQueryHandler : IRequestHandler<GetByIdLanguageQueryRequest, Result<GetByIdLanguageQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdLanguageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetByIdLanguageQueryResponse>> Handle(GetByIdLanguageQueryRequest request, CancellationToken cancellationToken)
    {
        var language = await _unitOfWork
            .ReadRepository<Domain.Entities.Concrete.Language, Guid>()
            .GetAll(tracking: false)
            .Where(l => l.Id == request.Id)
            .Select(l => new LanguageDetailDto(
                l.Id,
                l.Name,
                l.Code,
                l.Products.Count
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (language == null)
            return Result<GetByIdLanguageQueryResponse>.Failure("Language not found");

        return Result<GetByIdLanguageQueryResponse>.Success(new GetByIdLanguageQueryResponse(language));
    }
}