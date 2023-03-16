using Budget.Application.Commands;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentValidation;

namespace Budget.Application.Validators;

public class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseCommandValidator(IExpenseRepository repository)
    {
        RuleFor(x => x.Dto)
            .SetValidator(new ExpenseDtoValidator())
            .DependentRules(() => RuleFor(x => new { x.Dto.Description, x.Dto.Date })
                    .Must(x => !repository.ExistsExpenseWithDescriptionAndMonth(x.Description, x.Date.Month, x.Date.Year)));

        RuleFor(x => x.Dto.Id)
            .Equal(0);
    }
}