using ELibraryAPI.Application.Dtos;
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Product.UploadProductImage;

public record UploadProductImageCommandRequest : IRequest<Result<UploadProductImageCommandResponse>>
{
    public List<UploadFileDto> Files { get; init; } = [];
    public Guid ProductId { get; init; }
}
