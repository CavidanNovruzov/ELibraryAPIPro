using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.ShippingMethod.GetAllShippingMethod;

public sealed record GetAllShippingMethodQueryRequest(int Page = 1, int Size = 10) : IRequest<Result<GetAllShippingMethodQueryResponse>>;
