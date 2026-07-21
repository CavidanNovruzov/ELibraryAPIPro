using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Order.GetAllOrder;

public sealed record GetAllOrderQueryRequest(int Page = 1, int Size = 20) : IRequest<Result<GetAllOrderQueryResponse>>;