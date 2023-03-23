using Budget.Application.Requests;

using FluentValidation;

namespace Budget.Application.Validators;

public class GetIncomesByMonthRequestValidator : AbstractValidator<GetIncomesByMonthRequest>
{
    public GetIncomesByMonthRequestValidator()
    {
        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12);

        RuleFor(x => x.Year)
            .InclusiveBetween(1, 9999);
    }
}