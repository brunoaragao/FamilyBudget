using Budget.Application.Dtos;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetIncomesQueryHandler : IRequestHandler<GetIncomesQuery, Result<IEnumerable<IncomeDto>>>
{
    private readonly IIncomeRepository _repository;

    public GetIncomesQueryHandler(IIncomeRepository repository)
    {
        _repository = repository;
    }

    public Task<Result<IEnumerable<IncomeDto>>> Handle(GetIncomesQuery request, CancellationToken cancellationToken)
    {
        var incomes = _repository.GetIncomes();

        var dtos = incomes.Select(e => new IncomeDto
        {
            Id = e.Id,
            Amount = e.Amount,
            Date = e.Date,
            Description = e.Description
        });

        return Task.FromResult(Result.Ok(dtos));
    }
}