using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.ShippingMethod.DeleteShippingMethod;

public sealed record DeleteShippingMethodCommandRequest(Guid Id) : IRequest<Result>;
