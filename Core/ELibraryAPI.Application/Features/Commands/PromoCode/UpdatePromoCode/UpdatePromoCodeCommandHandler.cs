using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.PromoCode.UpdatePromoCode;

public sealed class UpdatePromoCodeCommandHandler : IRequestHandler<UpdatePromoCodeCommandRequest, Result<UpdatePromoCodeCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePromoCodeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdatePromoCodeCommandResponse>> Handle(UpdatePromoCodeCommandRequest request, CancellationToken ct)
    {
        var readRepo = _unitOfWork.ReadRepository<Domain.Entities.Concrete.PromoCode, Guid>();
        var promoCode = await readRepo.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (promoCode == null) return Result<UpdatePromoCodeCommandResponse>.Failure("Not found.");

        var normalizedCode = request.Code.Trim().ToUpper();

        if (promoCode.Code != normalizedCode)
        {
            bool exists = await readRepo.ExistsAsync(x => x.Code == normalizedCode && x.Id != request.Id, false, ct);
            if (exists) return Result<UpdatePromoCodeCommandResponse>.Failure("Code already exists.");
        }

        if (request.UsageLimit < promoCode.UsageCount)
        {
            return Result<UpdatePromoCodeCommandResponse>.Failure($"Usage limit cannot be lower than current usage count ({promoCode.UsageCount}).");
        }

        _mapper.Map(request, promoCode);
        promoCode.Code = normalizedCode;

        await _unitOfWork.SaveAsync(ct);

        return Result<UpdatePromoCodeCommandResponse>.Success(new(promoCode.Id), "Updated.");
    }
}