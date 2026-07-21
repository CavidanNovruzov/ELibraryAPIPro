using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Category.UpdateCategory;

public sealed record UpdateCategoryCommandRequest(
    Guid Id,
    string Name
) : IRequest<Result<UpdateCategoryCommandResponse>>;
