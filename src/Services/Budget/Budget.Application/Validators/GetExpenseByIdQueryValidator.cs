using Budget.Application.Queries;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentValidation;

namespace Budget.Application.Validators;

public class GetExpenseByIdQueryValidator : AbstractValidator<GetExpenseByIdQuery>
{
    public GetExpenseByIdQueryValidator(IExpenseRepository repository)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .DependentRules(() => RuleFor(x => x.Id)
                    .Must(id => repository.ExistsExpenseWithId(id)));
    }
}