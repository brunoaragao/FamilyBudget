using Budget.Application.Queries;

using FluentValidation;

namespace Budget.Application.Validators;

public class GetExpensesByDescriptionSnippetQueryValidator : AbstractValidator<GetExpensesByDescriptionSnippetQuery>
{
    public GetExpensesByDescriptionSnippetQueryValidator()
    {
        RuleFor(x => x.DescriptionSnippet)
            .NotEmpty();
    }
}