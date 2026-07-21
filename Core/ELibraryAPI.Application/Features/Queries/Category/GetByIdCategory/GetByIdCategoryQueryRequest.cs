using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Category.GetByIdCategory;

public sealed record GetByIdCategoryQueryRequest(Guid Id) : IRequest<Result<GetByIdCategoryQueryResponse>>;
