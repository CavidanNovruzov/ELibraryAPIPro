using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.SubCategory.GetAllSubCategory;

public sealed record GetAllSubCategoryQueryRequest(int Page = 1, int Size = 10) : IRequest<Result<GetAllSubCategoryQueryResponse>>;