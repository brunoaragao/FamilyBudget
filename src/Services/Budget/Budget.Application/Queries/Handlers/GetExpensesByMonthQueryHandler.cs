using Budget.Application.Dtos;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetExpensesByMonthQueryHandler : IRequestHandler<GetExpensesByMonthQuery, Result<IEnumerable<ExpenseDto>>>
{
    private readonly IExpenseRepository _repository;
    private readonly IValidator<GetExpensesByMonthQuery> _validator;

    public GetExpensesByMonthQueryHandler(IExpenseRepository repository, IValidator<GetExpensesByMonthQuery> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public Task<Result<IEnumerable<ExpenseDto>>> Handle(GetExpensesByMonthQuery request, CancellationToken cancellationToken)
    {
        // TODO: Move validation to a separate behavior
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Task.FromResult(Result.Fail<IEnumerable<ExpenseDto>>(validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var expenses = _repository.GetExpensesByMonth(request.Month, request.Year);

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