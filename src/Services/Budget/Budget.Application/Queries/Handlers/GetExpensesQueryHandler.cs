using Budget.Application.Dtos;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, Result<IEnumerable<ExpenseDto>>>
{
    private readonly IExpenseRepository _repository;

    public GetExpensesQueryHandler(IExpenseRepository repository)
    {
        _repository = repository;
    }

    public Task<Result<IEnumerable<ExpenseDto>>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var expenses = _repository.GetExpenses();

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