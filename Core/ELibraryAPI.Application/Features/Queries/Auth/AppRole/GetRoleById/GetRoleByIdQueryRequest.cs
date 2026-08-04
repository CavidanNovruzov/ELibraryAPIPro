using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppRole.GetRoleById;

public sealed record GetRoleByIdQueryRequest(Guid RoleId)
    : IRequest<Result<GetRoleByIdQueryResponse>>, ICacheable
{
    public string CacheKey => $"role:detail:{RoleId}";
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromHours(1);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(20);
}
