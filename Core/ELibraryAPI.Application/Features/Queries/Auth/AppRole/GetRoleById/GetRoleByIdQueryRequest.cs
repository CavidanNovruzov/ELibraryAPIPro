using ELibraryAPI.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppRole.GetRoleById;

public sealed record GetRoleByIdQueryRequest(Guid Id)
   : IRequest<Result<GetRoleByIdQueryResponse>>;
