using ELibraryAPI.Application.Features.Queries.Publisher.GetByIdPublisher;
using FluentValidation;

namespace ELibraryAPI.Application.Validations.Publisher;

public sealed class GetByIdPublisherQueryValidator : AbstractValidator<GetByIdPublisherQueryRequest>
{
    public GetByIdPublisherQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty);
    }
}
