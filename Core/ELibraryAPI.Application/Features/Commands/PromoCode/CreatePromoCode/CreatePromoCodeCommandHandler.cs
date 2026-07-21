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
            return Result<CreatePromoCodeCommandResponse>.Failure("This promo code already exists.");

        if (request.StartDate < DateTime.UtcNow.Date)
            return Result<CreatePromoCodeCommandResponse>.Failure("Start date cannot be in the past.");

        if (request.StartDate >= request.EndDate)
            return Result<CreatePromoCodeCommandResponse>.Failure("Start date must be earlier than the end date.");

        var promoCode = _mapper.Map<Domain.Entities.Concrete.PromoCode>(request);

        promoCode.Code = normalizedCode;
        promoCode.UsageCount = 0;
        promoCode.IsActive = true;

        await writeRepository.AddAsync(promoCode, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreatePromoCodeCommandResponse>.Success(
            new CreatePromoCodeCommandResponse(promoCode.Id),
            "Promo code created successfully.");
    }
}