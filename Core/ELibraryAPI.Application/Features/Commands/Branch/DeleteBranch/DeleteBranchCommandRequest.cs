using ELibraryAPI.Application.Responses;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Branch.DeleteBranch;

public sealed record DeleteBranchCommandRequest(Guid Id) : IRequest<Result>;
