using Budget.Application.Dtos;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, Result<ExpenseDto>>
{
    private readonly IExpenseRepository _repository;
    private readonly IValidator<GetExpenseByIdQuery> _validator;

    public GetExpenseByIdQueryHandler(IExpenseRepository repository, IValidator<GetExpenseByIdQuery> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public Task<Result<ExpenseDto>> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        // TODO: Move validation to a separate behavior
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Task.FromResult(Result.Fail<ExpenseDto>(validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var expense = _repository.GetExpenseById(request.Id);

        var dto = new ExpenseDto
        {
            Id = expense.Id,
            Amount = expense.Amount,
            Date = expense.Date,
            Description = expense.Description,
            ExpenseCategory = (ExpenseCategoryDto)expense.ExpenseCategory.Id
        };

        return Task.FromResult(Result.Ok(dto));
    }
}