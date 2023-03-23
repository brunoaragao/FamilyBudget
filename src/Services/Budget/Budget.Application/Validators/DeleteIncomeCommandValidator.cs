using Budget.Application.Requests;

using FluentValidation;

namespace Budget.Application.Validators;

public class DeleteIncomeRequestValidator : AbstractValidator<DeleteIncomeRequest>
{
    public DeleteIncomeRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}