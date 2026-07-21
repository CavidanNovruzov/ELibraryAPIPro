
using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Permission.GetAllPermission;

public sealed record GetAllPermissionQueryRequest : IRequest<Result<List<GetAllPermissionQueryResponse>>>;