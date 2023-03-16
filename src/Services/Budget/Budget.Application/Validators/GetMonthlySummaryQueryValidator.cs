using Budget.Application.Queries;

using FluentValidation;

namespace Budget.Application.Validators;

public class GetMonthlySummaryQueryValidator : AbstractValidator<GetMonthlySummaryQuery>
{
    public GetMonthlySummaryQueryValidator()
    {
        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12);

        RuleFor(x => x.Year)
            .InclusiveBetween(1, 9999);
    }
}