using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Order.GetByIdOrder;

public sealed record GetByIdOrderQueryRequest(Guid Id) : IRequest<Result<GetByIdOrderQueryResponse>>;
