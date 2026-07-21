using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Queries.Permission.GetByIdPermission;

public sealed record GetByIdPermissionQueryRequest(int Id) : IRequest<Result<GetByIdPermissionQueryResponse>>;
