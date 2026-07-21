using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.PaymentMethod.GetAllPaymentMethod;

public sealed record GetAllPaymentMethodQueryRequest(int Page = 1, int Size = 20) : IRequest<Result<GetAllPaymentMethodQueryResponse>>;
