using Budget.Application.Dtos;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetExpensesByDescriptionSnippetQueryHandler : IRequestHandler<GetExpensesByDescriptionSnippetQuery, Result<IEnumerable<ExpenseDto>>>
{
    private readonly IExpenseRepository _repository;
    private readonly IValidator<GetExpensesByDescriptionSnippetQuery> _validator;

    public GetExpensesByDescriptionSnippetQueryHandler(IExpenseRepository repository, IValidator<GetExpensesByDescriptionSnippetQuery> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public Task<Result<IEnumerable<ExpenseDto>>> Handle(GetExpensesByDescriptionSnippetQuery request, CancellationToken cancellationToken)
    {
        // TODO: Move validation to a separate behavior
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Task.FromResult(Result.Fail<IEnumerable<ExpenseDto>>(validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var expenses = _repository.GetExpensesByDescriptionSnippet(request.DescriptionSnippet);

        var dtos = expenses.Select(e => new ExpenseDto
        {
            Id = e.Id,
            Amount = e.Amount,
            Date = e.Date,
            Description = e.Description,
            ExpenseCategory = (ExpenseCategoryDto)e.ExpenseCategory.Id
        });

        return Task.FromResult(Result.Ok(dtos));
    }
}