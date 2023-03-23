using Budget.Application.Requests;

using FluentValidation;

namespace Budget.Application.Validators;

public class UpdateExpenseRequestValidator : AbstractValidator<UpdateExpenseRequest>
{
    public UpdateExpenseRequestValidator()
    {
        RuleFor(x => x.Dto)
            .SetValidator(new ExpenseDtoValidator());

        RuleFor(x => x.Dto.Id)
            .GreaterThan(0);
    }
}