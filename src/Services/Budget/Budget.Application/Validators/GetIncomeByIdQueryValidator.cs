using Budget.Application.Requests;

using FluentValidation;

namespace Budget.Application.Validators;

public class GetIncomeByIdRequestValidator : AbstractValidator<GetIncomeByIdRequest>
{
    public GetIncomeByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}