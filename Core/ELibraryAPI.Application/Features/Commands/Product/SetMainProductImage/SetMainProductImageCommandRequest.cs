using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Product.SetMainProductImage;

public sealed record SetMainProductImageCommandRequest(Guid ProductId, Guid ImageId) : IRequest<Result>;
