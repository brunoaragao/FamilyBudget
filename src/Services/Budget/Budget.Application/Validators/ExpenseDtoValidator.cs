using Budget.Application.Dtos;

using FluentValidation;

namespace Budget.Application.Validators;

public class ExpenseDtoValidator : AbstractValidator<ExpenseDto>
{
    public ExpenseDtoValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.Category)
            .IsInEnum();
    }
}