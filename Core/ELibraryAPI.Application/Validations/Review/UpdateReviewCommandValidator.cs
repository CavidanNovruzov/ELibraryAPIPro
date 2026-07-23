using ELibraryAPI.Application.Features.Commands.Review.UpdateReview;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Review;

public sealed class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommandRequest>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID-si boş ola bilməz.");

        RuleFor(x => x.Comment).NotEmpty().WithMessage("rəyi boş ola bilməz.").MaximumLength(2000).WithMessage("rəyi maksimum {MaxLength} simvol ola bilər.");
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Məhsul ID-si boş ola bilməz.");
        RuleFor(x => x.Rating).InclusiveBetween(1,5);
        RuleFor(x => x.UserId).NotEmpty().WithMessage("İstifadəçi ID-si boş ola bilməz.");
    }
}
