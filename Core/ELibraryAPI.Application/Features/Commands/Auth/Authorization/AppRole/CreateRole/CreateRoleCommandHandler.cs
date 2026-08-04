

using ELibraryAPI.Application.Abstractions.Services.Caching;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Authorization.AppRole.CreateRole;

public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommandRequest, Result<CreateRoleCommandResponse>>
{
    private readonly IUnitOfWork _uow;
    private readonly ICacheService _cacheService;

    public CreateRoleCommandHandler(IUnitOfWork uow, ICacheService cacheService)
    {
        _uow = uow;
        _cacheService = cacheService;
    }

    public async Task<Result<CreateRoleCommandResponse>> Handle(CreateRoleCommandRequest request, CancellationToken ct)
    {
        var writeRepo = _uow.WriteRepository<Domain.Entities.Concrete.Auth.AppRole, Guid>();

        var role = new Domain.Entities.Concrete.Auth.AppRole
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            NormalizedName = request.Name.ToUpperInvariant()
        };

        await writeRepo.AddAsync(role, ct);
        await _uow.SaveAsync(ct);

        await _cacheService.RemoveAsync("roles:all", ct);

        return Result<CreateRoleCommandResponse>.Success(new CreateRoleCommandResponse(role.Id));
    }
}