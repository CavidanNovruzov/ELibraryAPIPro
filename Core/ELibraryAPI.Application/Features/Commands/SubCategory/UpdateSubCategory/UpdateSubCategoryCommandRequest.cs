using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.SubCategory.UpdateSubCategory;

public sealed record UpdateSubCategoryCommandRequest(
    Guid Id,
    Guid CategoryId,
    string Name
) : IRequest<Result<UpdateSubCategoryCommandResponse>>;
