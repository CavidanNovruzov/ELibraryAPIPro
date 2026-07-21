using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Category.DeleteCategory;

public sealed record DeleteCategoryCommandRequest(Guid Id) : IRequest<Result>;
