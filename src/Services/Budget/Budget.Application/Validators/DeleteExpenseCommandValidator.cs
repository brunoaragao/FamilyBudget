using Budget.Application.Requests;

using FluentValidation;

namespace Budget.Application.Validators;

public class DeleteExpenseRequestValidator : AbstractValidator<DeleteExpenseRequest>
{
    public DeleteExpenseRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}