using ELibraryAPI.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.UpdateUserByAdmin;

public sealed record UpdateUserByAdminCommandRequest(
  Guid Id,
  string FirstName,
  string LastName,
  bool IsActive,
  bool EmailConfirmed) : IRequest<Result>;
