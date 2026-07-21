using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.PaymentMethod.UpdatePaymentMethod;

public sealed class UpdatePaymentMethodCommandHandler : IRequestHandler<UpdatePaymentMethodCommandRequest, Result<UpdatePaymentMethodCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePaymentMethodCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<UpdatePaymentMethodCommandResponse>> Handle(UpdatePaymentMethodCommandRequest request, CancellationToken ct)
    {
        var readRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.PaymentMethod, Guid>();
        var writeRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.PaymentMethod, Guid>();

        var paymentMethod = await readRepository.GetByIdAsync(request.Id, tracking: true, ct: ct);

        if (paymentMethod == null)
        {
            return Result<UpdatePaymentMethodCommandResponse>.Failure("Payment method not found.");
        }

        if (paymentMethod.Name.ToLower() != request.Name.Trim().ToLower())
        {
            var isDuplicate = await readRepository.ExistsAsync(
                x => x.Name.ToLower() == request.Name.Trim().ToLower() && x.Id != request.Id,
                tracking: false,
                ct: ct);

            if (isDuplicate)
            {
                return Result<UpdatePaymentMethodCommandResponse>.Failure("Another payment method with this name already exists.");
            }
        }

        _mapper.Map(request, paymentMethod);

        writeRepository.Update(paymentMethod);
        await _unitOfWork.SaveAsync(ct);

        return Result<UpdatePaymentMethodCommandResponse>.Success(
            new UpdatePaymentMethodCommandResponse(paymentMethod.Id),
            "Payment method updated successfully.");
    }
}