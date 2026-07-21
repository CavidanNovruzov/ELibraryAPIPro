namespace ELibraryAPI.Application.Features.Queries.Language.GetByIdLanguage;

public sealed record GetByIdLanguageQueryResponse(
    LanguageDetailDto Language
);

public sealed record LanguageDetailDto(
    Guid Id,
    string Name,
    string Code,
    int ProductCount
); 
