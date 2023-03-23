using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetExpensesRequestHandler : IRequestHandler<GetExpensesRequest, Result<IEnumerable<ExpenseDto>>>
{
    private readonly IExpenseRepository _repository;
    private readonly IMapper _mapper;

    public GetExpensesRequestHandler(IExpenseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<Result<IEnumerable<ExpenseDto>>> Handle(GetExpensesRequest request, CancellationToken cancellationToken) => Task.Run<Result<IEnumerable<ExpenseDto>>>(() =>
    {
        var expenses = _repository.GetExpenses();
        var dtos = _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        return Result.Ok(dtos);
    });
}