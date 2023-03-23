using Budget.Application.Dtos;

using FluentValidation;

namespace Budget.Application.Validators;

public class IncomeDtoValidator : AbstractValidator<IncomeDto>
{
    public IncomeDtoValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .NotEmpty();
    }
}