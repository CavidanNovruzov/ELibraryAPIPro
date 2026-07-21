using System.Collections.Generic;
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Category.GetAllCategory;

public sealed record GetAllCategoryQueryResponse(
    List<CategoryListDto> Categories,
    int TotalCount,
    int Page,
    int Size,
    int TotalPages
);

public sealed record CategoryListDto(Guid Id, string Name, int SubCategoryCount, int ProductCount);