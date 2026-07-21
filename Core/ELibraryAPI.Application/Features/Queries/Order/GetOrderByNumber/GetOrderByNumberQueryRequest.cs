using ELibraryAPI.Application.Responses;
using MediatR;


namespace ELibraryAPI.Application.Features.Queries.Order.GetOrderByNumber;

public sealed record GetOrderByNumberQueryRequest(string OrderNumber) : IRequest<Result<GetOrderByNumberQueryResponse>>;
