using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.ShippingMethod.CreateShippingMethod;

public sealed class CreateShippingMethodCommandHandler : IRequestHandler<CreateShippingMethodCommandRequest, Result<CreateShippingMethodCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateShippingMethodCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreateShippingMethodCommandResponse>> Handle(CreateShippingMethodCommandRequest request, CancellationToken ct)
    {
        var readRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.ShippingMethod, Guid>();
        var writeRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.ShippingMethod, Guid>();

        var isNameExists = await readRepository.ExistsAsync(
            x => x.Name.ToLower() == request.Name.Trim().ToLower(),
            tracking: false,
            ct: ct);

        if (isNameExists)
        {
            return Result<CreateShippingMethodCommandResponse>.Failure("Bu adda çatdırılma metodu artıq mövcuddur.");
        }

        if (request.Price < 0)
        {
            return Result<CreateShippingMethodCommandResponse>.Failure("Çatdırılma qiyməti mənfi ola bilməz.");
        }

        var shippingMethod = _mapper.Map<Domain.Entities.Concrete.ShippingMethod>(request);
        shippingMethod.Name = request.Name.Trim();

        await writeRepository.AddAsync(shippingMethod, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreateShippingMethodCommandResponse>.Success(
            new CreateShippingMethodCommandResponse(shippingMethod.Id),
            "Əməliyyat uğurla tamamlandı.");
    }
}