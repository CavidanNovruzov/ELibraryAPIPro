using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;


namespace ELibraryAPI.Application.Features.Commands.Auth.AppUser.DeleteUser;

public sealed class DeleteUserCommandHandler(IUnitOfWork uow) : IRequestHandler<DeleteUserCommandRequest, Result>
{
        public async Task<Result> Handle(DeleteUserCommandRequest request, CancellationToken ct)
        {
            var result = await uow.WriteRepository<Domain.Entities.Concrete.Auth.AppUser, Guid>().RemoveAsync(request.Id, ct);

            if (!result)
                return Result.NotFound("İstifadəçi tapılmadı.");

            await uow.SaveAsync(ct);

            return Result.Success("İstifadəçi uğurla silindi.");
        }
    
}
