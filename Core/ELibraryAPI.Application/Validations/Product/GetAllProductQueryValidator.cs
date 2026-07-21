using ELibraryAPI.Application.Features.Queries.Product.GetAllProduct;
using FluentValidation;


namespace ELibraryAPI.Application.Validations.Product
{
    public sealed class GetAllProductQueryValidator : AbstractValidator<GetAllProductQueryRequest>
    {
        public GetAllProductQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

            RuleFor(x => x.Size)
                .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100."); 

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum price cannot be negative.")
                .When(x => x.MinPrice.HasValue);

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Maximum price cannot be negative.")
                .When(x => x.MaxPrice.HasValue);

            RuleFor(x => x)
                .Must(x => x.MaxPrice >= x.MinPrice)
                .WithMessage("Maximum price must be greater than or equal to minimum price.")
                .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);

            RuleFor(x => x.Search)
                .MinimumLength(2).WithMessage("Search term must be at least 2 characters long.")
                .When(x => !string.IsNullOrWhiteSpace(x.Search));

            RuleFor(x => x.SortBy)
                .Must(s => new[] { "PriceAsc", "PriceDesc", "Newest", "TopRated", null }.Contains(s))
                .WithMessage("Invalid sort option.");
        }
    }
}
