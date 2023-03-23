using Budget.Application.Requests;

using FluentValidation;

namespace Budget.Application.Validators;

public class UpdateIncomeRequestValidator : AbstractValidator<UpdateIncomeRequest>
{
    public UpdateIncomeRequestValidator()
    {
        RuleFor(x => x.Dto)
            .SetValidator(new IncomeDtoValidator());

        RuleFor(x => x.Dto.Id)
            .GreaterThan(0);
    }
}