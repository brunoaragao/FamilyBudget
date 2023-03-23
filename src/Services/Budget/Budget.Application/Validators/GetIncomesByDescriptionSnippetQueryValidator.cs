using Budget.Application.Requests;

using FluentValidation;

namespace Budget.Application.Validators;

public class GetIncomesByDescriptionSnippetRequestValidator : AbstractValidator<GetIncomesByDescriptionSnippetRequest>
{
    public GetIncomesByDescriptionSnippetRequestValidator()
    {
        RuleFor(x => x.DescriptionSnippet)
            .NotEmpty();
    }
}