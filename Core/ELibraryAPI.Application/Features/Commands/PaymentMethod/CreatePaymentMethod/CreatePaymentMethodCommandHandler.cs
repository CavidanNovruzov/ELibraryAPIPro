using AutoMapper;
using ELibraryAPI.Application.Responses;
using ELibraryAPI.Application.UnitOfWork;
using MediatR;

namespace ELibraryAPI.Application.Features.Commands.PaymentMethod.CreatePaymentMethod;

public sealed class CreatePaymentMethodCommandHandler : IRequestHandler<CreatePaymentMethodCommandRequest, Result<CreatePaymentMethodCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreatePaymentMethodCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CreatePaymentMethodCommandResponse>> Handle(CreatePaymentMethodCommandRequest request, CancellationToken ct)
    {
        var readRepository = _unitOfWork.ReadRepository<Domain.Entities.Concrete.PaymentMethod, Guid>();
        var writeRepository = _unitOfWork.WriteRepository<Domain.Entities.Concrete.PaymentMethod, Guid>();

        var isExists = await readRepository.ExistsAsync(
            x => x.Name.ToLower() == request.Name.Trim().ToLower(),
            tracking: false,
            ct: ct);

        if (isExists)
        {
            return Result<CreatePaymentMethodCommandResponse>.Failure("This payment method already exists.");
        }

        var paymentMethod = _mapper.Map<Domain.Entities.Concrete.PaymentMethod>(request);

        await writeRepository.AddAsync(paymentMethod, ct);
        await _unitOfWork.SaveAsync(ct);

        return Result<CreatePaymentMethodCommandResponse>.Success(
            new CreatePaymentMethodCommandResponse(paymentMethod.Id),
            "Payment method created successfully.");
    }
}