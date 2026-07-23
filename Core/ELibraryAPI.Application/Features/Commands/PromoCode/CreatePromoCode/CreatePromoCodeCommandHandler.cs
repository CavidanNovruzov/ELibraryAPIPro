using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.PromoCode.CreatePromoCode;

public sealed class CreatePromoCodeCommandHandler : IRequestHandler<CreatePromoCodeCommandRequest, Result<CreatePromoCodeCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePromoCodeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreatePromoCodeCommandResponse>> Handle(CreatePromoCodeCommandRequest request, CancellationToken ct)
    {
        var readRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.PromoCode, Guid>();
        var writeRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.PromoCode, Guid>();

        var normalizedCode = request.Code.Trim().ToUpper();
        var isCodeExists = await readRepository.ExistsAsync(
            x => x.Code == normalizedCode,
            tracking: false,
            ct: ct);

        if (isCodeExists)
            return Result<CreatePromoCodeCommandResponse>.Failure("Bu promo kod artıq mövcuddur.");

        if (request.StartDate < DateTime.UtcNow.Date)
            return Result<CreatePromoCodeCommandResponse>.Failure("Başlanğıc tarixi keçmişdə ola bilməz.");

        if (request.StartDate >= request.EndDate)
            return Result<CreatePromoCodeCommandResponse>.Failure("Başlanğıc tarixi bitmə tarixindən əvvəl olmalıdır.");

        var promoCode = _mapper.Map<Domain.Entities.Concrete.PromoCode>(request);

        promoCode.Code = normalizedCode;
        promoCode.UsageCount = 0;
        promoCode.IsActive = true;

        await writeRepository.AddAsync(promoCode, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreatePromoCodeCommandResponse>.Success(
            new CreatePromoCodeCommandResponse(promoCode.Id),
            "Əməliyyat uğurla tamamlandı.");
    }
}