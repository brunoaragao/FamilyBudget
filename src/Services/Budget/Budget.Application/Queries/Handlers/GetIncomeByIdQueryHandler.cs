using Budget.Application.Dtos;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetIncomeByIdQueryHandler : IRequestHandler<GetIncomeByIdQuery, Result<IncomeDto>>
{
    private readonly IIncomeRepository _repository;
    private readonly IValidator<GetIncomeByIdQuery> _validator;

    public GetIncomeByIdQueryHandler(IIncomeRepository repository, IValidator<GetIncomeByIdQuery> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public Task<Result<IncomeDto>> Handle(GetIncomeByIdQuery request, CancellationToken cancellationToken)
    {
        // TODO: Move validation to a separate behavior
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Task.FromResult(Result.Fail<IncomeDto>(validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var income = _repository.GetIncomeById(request.Id);

        var dto = new IncomeDto
        {
            Id = income.Id,
            Amount = income.Amount,
            Date = income.Date,
            Description = income.Description
        };

        return Task.FromResult(Result.Ok(dto));
    }
}