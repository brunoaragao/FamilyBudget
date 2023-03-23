using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetIncomesRequestHandler : IRequestHandler<GetIncomesRequest, Result<IEnumerable<IncomeDto>>>
{
    private readonly IIncomeRepository _repository;
    private readonly IMapper _mapper;

    public GetIncomesRequestHandler(IIncomeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<Result<IEnumerable<IncomeDto>>> Handle(GetIncomesRequest request, CancellationToken cancellationToken) => Task.Run<Result<IEnumerable<IncomeDto>>>(() =>
    {
        var incomes = _repository.GetIncomes();
        var dtos = _mapper.Map<IEnumerable<IncomeDto>>(incomes);
        return Result.Ok(dtos);
    });
}