using Budget.Application.Requests;

using FluentValidation;

namespace Budget.Application.Validators;

public class GetExpensesByDescriptionSnippetRequestValidator : AbstractValidator<GetExpensesByDescriptionSnippetRequest>
{
    public GetExpensesByDescriptionSnippetRequestValidator()
    {
        RuleFor(x => x.DescriptionSnippet)
            .NotEmpty();
    }
}