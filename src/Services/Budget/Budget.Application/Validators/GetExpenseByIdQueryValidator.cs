using Budget.Application.Requests;

using FluentValidation;

namespace Budget.Application.Validators;

public class GetExpenseByIdRequestValidator : AbstractValidator<GetExpenseByIdRequest>
{
    public GetExpenseByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}