using Budget.Application.Dtos;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetIncomesByMonthQueryHandler : IRequestHandler<GetIncomesByMonthQuery, Result<IEnumerable<IncomeDto>>>
{
    private readonly IIncomeRepository _repository;
    private readonly IValidator<GetIncomesByMonthQuery> _validator;

    public GetIncomesByMonthQueryHandler(IIncomeRepository repository, IValidator<GetIncomesByMonthQuery> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public Task<Result<IEnumerable<IncomeDto>>> Handle(GetIncomesByMonthQuery request, CancellationToken cancellationToken)
    {
        // TODO: Move validation to a separate behavior
        var validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Task.FromResult(Result.Fail<IEnumerable<IncomeDto>>(validationResult.Errors.Select(x => x.ErrorMessage)));
        }

        var incomes = _repository.GetIncomesByMonth(request.Month, request.Year);

        var dtos = incomes.Select(i => new IncomeDto
        {
            Id = i.Id,
            Amount = i.Amount,
            Date = i.Date,
            Description = i.Description
        });

        return Task.FromResult(Result.Ok(dtos));
    }
}