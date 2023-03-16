using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.Application.Commands.Handlers;

public class UpdateIncomeCommandHandler : IRequestHandler<UpdateIncomeCommand, Result>
{
    private readonly IIncomeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateIncomeCommand> _validator;

    public UpdateIncomeCommandHandler(IIncomeRepository repository, IUnitOfWork unitOfWork, IValidator<UpdateIncomeCommand> validator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result> Handle(UpdateIncomeCommand request, CancellationToken cancellationToken)
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

        var income = _repository.GetIncomeById(id);

        if (income == null)
        {
            return Result.Fail($"Income with id {id} not found");
        }

        income.Update(_repository, amount, description, date);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Ok();
    }
}