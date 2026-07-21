using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.SubCategory.MergeSubCategories;

public sealed record MergeSubCategoriesCommandRequest(
    Guid SourceSubCategoryId,
    Guid TargetSubCategoryId
) : IRequest<Result<MergeSubCategoriesCommandResponse>>;