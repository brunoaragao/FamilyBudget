using Budget.Application.Requests;

using FluentValidation;

namespace Budget.Application.Validators;

public class CreateExpenseRequestValidator : AbstractValidator<CreateExpenseRequest>
{
    public CreateExpenseRequestValidator()
    {
        RuleFor(x => x.Dto)
            .SetValidator(new ExpenseDtoValidator());

        RuleFor(x => x.Dto.Id)
            .Equal(0);
    }
}