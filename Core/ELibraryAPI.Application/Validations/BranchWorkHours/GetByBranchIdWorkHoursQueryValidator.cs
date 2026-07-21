using ELibraryAPI.Application.Features.Queries.BranchWorkHours.GetAllBranchWorkHours;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELibraryAPI.Application.Validations.BranchWorkHours;

public sealed class GetByBranchIdWorkHoursQueryValidator : AbstractValidator<GetByBranchIdWorkHoursQueryRequest>
{
    public GetByBranchIdWorkHoursQueryValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Filial ID-si boş ola bilməz.")
            .NotEqual(Guid.Empty).WithMessage("Keçərli bir ID daxil edin.");
    }
}
