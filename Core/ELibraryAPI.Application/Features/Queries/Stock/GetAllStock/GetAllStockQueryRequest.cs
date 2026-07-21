using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Stock.GetAllStock;

public sealed record GetAllStockQueryRequest(int Page = 1, int Size = 10) : IRequest<Result<GetAllStockQueryResponse>>;