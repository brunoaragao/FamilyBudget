using Budget.Application.Queries;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentValidation;

namespace Budget.Application.Validators;

public class GetIncomeByIdQueryValidator : AbstractValidator<GetIncomeByIdQuery>
{
    public GetIncomeByIdQueryValidator(IIncomeRepository repository)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .DependentRules(() => RuleFor(x => x.Id)
                    .Must(id => repository.ExistsIncomeWithId(id)));
    }
}