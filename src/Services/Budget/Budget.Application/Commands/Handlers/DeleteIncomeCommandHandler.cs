using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.Application.Commands.Handlers;

public class DeleteIncomeCommandHandler : IRequestHandler<DeleteIncomeCommand, Result>
{
    private readonly IIncomeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteIncomeCommand> _validator;

    public DeleteIncomeCommandHandler(IIncomeRepository repository, IUnitOfWork unitOfWork, IValidator<DeleteIncomeCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
    {
        // TODO: Move validation to a separate behavior
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        var income = _repository.GetIncomeById(request.Id);

        _repository.DeleteIncome(income);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok();
    }
}