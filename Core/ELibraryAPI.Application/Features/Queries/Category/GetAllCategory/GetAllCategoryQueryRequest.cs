using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Category.GetAllCategory;

public sealed record GetAllCategoryQueryRequest(int Page = 1, int Size = 10) : IRequest<Result<GetAllCategoryQueryResponse>>, ICacheable
{
    public string CacheKey => "categories:all";
    public TimeSpan? CacheExpiry => TimeSpan.FromHours(1);

}