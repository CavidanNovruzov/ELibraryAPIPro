using ELibraryAPI.Application.Responses;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.Product.DeleteProductImage;

public sealed record DeleteProductImageCommandRequest(Guid Id) : IRequest<Result>;
