using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.Application.Commands.Handlers;

public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, Result>
{
    private readonly IExpenseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateExpenseCommand> _validator;

    public UpdateExpenseCommandHandler(IExpenseRepository repository, IUnitOfWork unitOfWork, IValidator<UpdateExpenseCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        // TODO: Move validation to a separate behavior
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        var id = request.Dto.Id;
        var amount = request.Dto.Amount;
        var description = request.Dto.Description;
        var date = request.Dto.Date;
        var expenseCategoryDto = request.Dto.ExpenseCategory;

        var expenseCategory = Enumeration.FromValue<ExpenseCategory>((int)expenseCategoryDto);

        var expense = _repository.GetExpenseById(id);

        expense.Update(_repository, amount, description, date, expenseCategory);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok();
    }
}