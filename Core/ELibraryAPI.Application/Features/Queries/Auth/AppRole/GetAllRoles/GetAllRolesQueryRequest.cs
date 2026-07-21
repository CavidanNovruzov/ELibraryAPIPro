using MediatR;
using ELibraryAPI.Application.Responses;

namespace ELibraryAPI.Application.Features.Queries.AppRole.GetAllRoles;

public sealed record GetAllRolesQueryRequest() : IRequest<Result<List<GetAllRolesQueryResponse>>>;
