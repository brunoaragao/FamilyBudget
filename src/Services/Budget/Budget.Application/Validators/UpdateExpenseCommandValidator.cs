using Budget.Application.Commands;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentValidation;

namespace Budget.Application.Validators;

public class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseCommandValidator(IExpenseRepository repository)
    {
        RuleFor(x => x.Dto)
            .SetValidator(new ExpenseDtoValidator())
            .DependentRules(() => RuleFor(x => new { x.Dto.Description, x.Dto.Date, x.Dto.Id })
                    .Must(x => !repository.ExistsAnotherExpenseWithDescriptionAndMonth(x.Id, x.Description, x.Date.Month, x.Date.Year)));

        RuleFor(x => x.Dto.Id)
            .GreaterThan(0)
            .DependentRules(() => RuleFor(x => x.Dto.Id)
                    .Must(id => repository.ExistsExpenseWithId(id)));
    }
}