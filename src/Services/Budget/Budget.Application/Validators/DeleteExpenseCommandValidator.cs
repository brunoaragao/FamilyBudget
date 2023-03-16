using Budget.Application.Commands;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentValidation;

namespace Budget.Application.Validators;

public class DeleteExpenseCommandValidator : AbstractValidator<DeleteExpenseCommand>
{
    public DeleteExpenseCommandValidator(IExpenseRepository repository)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .DependentRules(() => RuleFor(x => x.Id)
                    .Must(id => repository.ExistsExpenseWithId(id)));
    }
}