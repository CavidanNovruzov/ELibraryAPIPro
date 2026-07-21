using AutoMapper;
using ELibraryAPI.Application.Features.Commands.PaymentMethod.CreatePaymentMethod;
using ELibraryAPI.Application.Features.Commands.PaymentMethod.UpdatePaymentMethod;
using ELibraryAPI.Application.Features.Queries.PaymentMethod.GetAllPaymentMethod;
using ELibraryAPI.Domain.Entities.Concrete;


public class PaymentMethodProfile : Profile
{
    public PaymentMethodProfile()
    {
        CreateMap<CreatePaymentMethodCommandRequest, PaymentMethod>();
        CreateMap<PaymentMethod, CreatePaymentMethodCommandResponse>();
        CreateMap<UpdatePaymentMethodCommandRequest, PaymentMethod>();
        CreateMap<PaymentMethod, UpdatePaymentMethodCommandResponse>();

        CreateMap<PaymentMethod, PaymentMethodListDto>();

    }
}
