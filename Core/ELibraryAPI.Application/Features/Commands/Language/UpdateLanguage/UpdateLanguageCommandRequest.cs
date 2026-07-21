using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Language.UpdateLanguage;

public sealed record UpdateLanguageCommandRequest(
    Guid Id,
    string Code,
    string Name
) : IRequest<Result<UpdateLanguageCommandResponse>>;
