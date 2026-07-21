using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.SubCategory.CreateSubCategory;

public sealed record CreateSubCategoryCommandRequest(
    Guid CategoryId,
    string Name
) : IRequest<Result<CreateSubCategoryCommandResponse>>;
