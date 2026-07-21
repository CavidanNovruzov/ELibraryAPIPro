using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.SubCategory.GetByIdSubCategory;

public sealed record GetByIdSubCategoryQueryRequest(Guid Id) : IRequest<Result<GetByIdSubCategoryQueryResponse>>;
