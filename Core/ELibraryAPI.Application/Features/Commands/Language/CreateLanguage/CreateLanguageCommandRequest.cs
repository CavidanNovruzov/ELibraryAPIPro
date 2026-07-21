using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Language.CreateLanguage;

public sealed record CreateLanguageCommandRequest(
    string Code,
    string Name
) : IRequest<Result<CreateLanguageCommandResponse>>;
