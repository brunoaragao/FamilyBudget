using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.Application.Commands.Handlers;

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Result<int>>
{
    private readonly IExpenseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateExpenseCommand> _validator;

    public CreateExpenseCommandHandler(IExpenseRepository repository, IUnitOfWork unitOfWork, IValidator<CreateExpenseCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<int>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        // TODO: Move validation to a separate behavior
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result.Fail<int>(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        var amount = request.Dto.Amount;
        var description = request.Dto.Description;
        var date = request.Dto.Date;
        var expenseCategoryDto = request.Dto.ExpenseCategory;

        var expenseCategory = Enumeration.FromValue<ExpenseCategory>((int)expenseCategoryDto);
        var expense = new Expense(_repository, amount, description, date, expenseCategory);

        _repository.AddExpense(expense);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok(expense.Id);
    }
}