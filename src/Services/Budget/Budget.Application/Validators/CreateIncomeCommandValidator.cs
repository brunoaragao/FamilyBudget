using Budget.Application.Commands;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentValidation;

namespace Budget.Application.Validators;

public class CreateIncomeCommandValidator : AbstractValidator<CreateIncomeCommand>
{
    public CreateIncomeCommandValidator(IIncomeRepository repository)
    {
        RuleFor(x => x.Dto)
            .SetValidator(new IncomeDtoValidator())
            .DependentRules(() => RuleFor(x => new { x.Dto.Description, x.Dto.Date })
                    .Must(x => !repository.ExistsIncomeWithDescriptionAndMonth(x.Description, x.Date.Month, x.Date.Year)));

        RuleFor(x => x.Dto.Id)
            .Equal(0);
    }
}