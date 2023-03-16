using Budget.Application.Commands;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentValidation;

namespace Budget.Application.Validators;

public class UpdateIncomeCommandValidator : AbstractValidator<UpdateIncomeCommand>
{
    public UpdateIncomeCommandValidator(IIncomeRepository repository)
    {
        RuleFor(x => x.Dto)
            .SetValidator(new IncomeDtoValidator())
            .DependentRules(() => RuleFor(x => new { x.Dto.Description, x.Dto.Date, x.Dto.Id })
                    .Must(x => !repository.ExistsAnotherIncomeWithDescriptionAndMonth(x.Id, x.Description, x.Date.Month, x.Date.Year)));

        RuleFor(x => x.Dto.Id)
            .GreaterThan(0)
            .DependentRules(() => RuleFor(x => x.Dto.Id)
                    .Must(id => repository.ExistsIncomeWithId(id)));
    }
}