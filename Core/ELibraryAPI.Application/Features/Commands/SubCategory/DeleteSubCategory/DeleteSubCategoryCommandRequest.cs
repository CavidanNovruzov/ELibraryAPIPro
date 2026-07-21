using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.SubCategory.DeleteSubCategory;

public sealed record DeleteSubCategoryCommandRequest(Guid Id) : IRequest<Result>;
