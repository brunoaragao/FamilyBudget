using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.Application.Commands.Handlers;

public class CreateIncomeCommandHandler : IRequestHandler<CreateIncomeCommand, Result<int>>
{
    private readonly IIncomeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateIncomeCommand> _validator;

    public CreateIncomeCommandHandler(IIncomeRepository repository, IUnitOfWork unitOfWork, IValidator<CreateIncomeCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<int>> Handle(CreateIncomeCommand request, CancellationToken cancellationToken)
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
        var income = new Income(_repository, amount, description, date);

        _repository.AddIncome(income);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok(income.Id);
    }
}