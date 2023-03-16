using Budget.Application.Queries;

using FluentValidation;

namespace Budget.Application.Validators;

public class GetIncomesByDescriptionSnippetQueryValidator : AbstractValidator<GetIncomesByDescriptionSnippetQuery>
{
    public GetIncomesByDescriptionSnippetQueryValidator()
    {
        RuleFor(x => x.DescriptionSnippet)
            .NotEmpty();
    }
}