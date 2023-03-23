using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetIncomesByMonthRequestHandler : IRequestHandler<GetIncomesByMonthRequest, Result<IEnumerable<IncomeDto>>>
{
    private readonly IIncomeRepository _repository;
    private readonly IMapper _mapper;

    public GetIncomesByMonthRequestHandler(IIncomeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<Result<IEnumerable<IncomeDto>>> Handle(GetIncomesByMonthRequest request, CancellationToken cancellationToken) => Task.Run<Result<IEnumerable<IncomeDto>>>(() =>
    {
        var incomes = _repository.GetIncomesByMonth(request.Month, request.Year);
        var dtos = _mapper.Map<IEnumerable<IncomeDto>>(incomes);
        return Result.Ok(dtos);
    });
}