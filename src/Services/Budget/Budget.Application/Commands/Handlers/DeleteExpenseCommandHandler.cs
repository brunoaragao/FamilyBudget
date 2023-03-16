using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.Application.Commands.Handlers;

public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, Result>
{
    private readonly IExpenseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteExpenseCommand> _validator;

    public DeleteExpenseCommandHandler(IExpenseRepository repository, IUnitOfWork unitOfWork, IValidator<DeleteExpenseCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        // TODO: Move validation to a separate behavior
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        var expense = _repository.GetExpenseById(request.Id);

        _repository.DeleteExpense(expense);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok();
    }
}