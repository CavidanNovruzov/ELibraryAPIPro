using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Queries.Auth.AppRole.GetRoleById;

public sealed record GetRoleByIdQueryResponse(
   Guid Id,
   string Name,
   List<string> Permissions);
