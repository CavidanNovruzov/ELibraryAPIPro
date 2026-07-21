

using ELibraryAPI.Application.Features.Commands.Auth.Authorization.AppRole.CreateRole;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.Auth.Roles.AppRole.CreateRole;

public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommandRequest, Result<CreateRoleCommandResponse>>
{
    private readonly IUnitOfWork _uow;
    public CreateRoleCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Result<CreateRoleCommandResponse>> Handle(CreateRoleCommandRequest request, CancellationToken ct)
    {
       var writeRepo=_uow.WriteRepository<Domain.Entities.Concrete.Auth.AppRole, Guid>();

        var role = new Domain.Entities.Concrete.Auth.AppRole
        {
            Id=Guid.NewGuid(),
            Name=request.Name
        };

        await writeRepo.AddAsync(role);

        await _uow.SaveAsync(ct);

        return Result<CreateRoleCommandResponse>.Success(new CreateRoleCommandResponse(role.Id));

    }
}