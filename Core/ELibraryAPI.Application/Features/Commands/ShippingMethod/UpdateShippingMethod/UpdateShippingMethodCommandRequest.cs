using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.ShippingMethod.UpdateShippingMethod;

public sealed record UpdateShippingMethodCommandRequest(
    Guid Id,
    string Name,
    decimal Price
) : IRequest<Result<UpdateShippingMethodCommandResponse>>;
