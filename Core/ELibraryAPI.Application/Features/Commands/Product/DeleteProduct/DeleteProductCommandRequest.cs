using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Product.DeleteProduct;

public sealed record DeleteProductCommandRequest(Guid Id) : IRequest<Result>;
