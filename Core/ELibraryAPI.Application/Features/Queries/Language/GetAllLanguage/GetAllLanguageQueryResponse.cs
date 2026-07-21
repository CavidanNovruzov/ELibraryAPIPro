

namespace ELibraryAPI.Application.Features.Queries.Language.GetAllLanguage;

public sealed record GetAllLanguageQueryResponse(
    List<LanguageListDto> Languages,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record LanguageListDto(Guid Id, string Name, string Code, int ProductCount);