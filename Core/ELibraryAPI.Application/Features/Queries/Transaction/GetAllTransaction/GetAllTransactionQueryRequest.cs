using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Transaction.GetAllTransaction;

public sealed record GetAllTransactionQueryRequest(int Page = 1, int Size = 20) : IRequest<Result<GetAllTransactionQueryResponse>>;