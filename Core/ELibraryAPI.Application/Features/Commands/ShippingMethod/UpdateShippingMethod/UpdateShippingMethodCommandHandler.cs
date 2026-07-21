using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.ShippingMethod.UpdateShippingMethod;

public sealed class UpdateShippingMethodCommandHandler : IRequestHandler<UpdateShippingMethodCommandRequest, Result<UpdateShippingMethodCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateShippingMethodCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdateShippingMethodCommandResponse>> Handle(UpdateShippingMethodCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.ShippingMethod, Guid>();

        var shippingMethod = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (shippingMethod == null)
            return Result<UpdateShippingMethodCommandResponse>.Failure("Shipping method not found.");

        var normalizedName = request.Name.Trim().ToLower();
        if (shippingMethod.Name.ToLower() != normalizedName)
        {
            var exists = await readRepo.ExistsAsync(x => x.Name.ToLower() == normalizedName, false, ct);
            if (exists) return Result<UpdateShippingMethodCommandResponse>.Failure("Name already exists.");
        }

        _mapper.Map(request, shippingMethod);
        shippingMethod.Name = request.Name.Trim();

        await _unitOfWork.SaveAsync(ct);

        return Result<UpdateShippingMethodCommandResponse>.Success(new(shippingMethod.Id), "Updated successfully.");
    }
}