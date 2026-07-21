using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.ShippingMethod.CreateShippingMethod;

public sealed record CreateShippingMethodCommandRequest(
    string Name,
    decimal Price
) : IRequest<Result<CreateShippingMethodCommandResponse>>;
