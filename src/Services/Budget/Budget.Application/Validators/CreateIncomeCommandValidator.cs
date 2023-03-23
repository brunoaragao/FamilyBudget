using Budget.Application.Requests;

using FluentValidation;

namespace Budget.Application.Validators;

public class CreateIncomeRequestValidator : AbstractValidator<CreateIncomeRequest>
{
    public CreateIncomeRequestValidator()
    {
        RuleFor(x => x.Dto)
            .SetValidator(new IncomeDtoValidator());

        RuleFor(x => x.Dto.Id)
            .Equal(0);
    }
}