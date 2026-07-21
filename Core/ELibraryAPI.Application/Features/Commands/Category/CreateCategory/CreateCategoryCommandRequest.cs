using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Category.CreateCategory;

public sealed record CreateCategoryCommandRequest(
    string Name
) : IRequest<Result<CreateCategoryCommandResponse>>;
