using Budget.Application.Commands;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentValidation;

namespace Budget.Application.Validators;

public class DeleteIncomeCommandValidator : AbstractValidator<DeleteIncomeCommand>
{
    public DeleteIncomeCommandValidator(IIncomeRepository repository)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .DependentRules(() => RuleFor(x => x.Id)
                    .Must(id => repository.ExistsIncomeWithId(id)));
    }
}