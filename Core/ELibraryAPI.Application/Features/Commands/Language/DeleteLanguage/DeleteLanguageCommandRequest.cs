using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Language.DeleteLanguage;

public sealed record DeleteLanguageCommandRequest(Guid Id) : IRequest<Result>;
