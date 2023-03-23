using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Errors;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetExpenseByIdRequestHandler : IRequestHandler<GetExpenseByIdRequest, Result<ExpenseDto>>
{
    private readonly IExpenseRepository _repository;
    private readonly IMapper _mapper;

    public GetExpenseByIdRequestHandler(IExpenseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<Result<ExpenseDto>> Handle(GetExpenseByIdRequest request, CancellationToken cancellationToken) => Task.Run<Result<ExpenseDto>>(() =>
    {
        if (!_repository.ExistsExpenseWithId(request.Id))
            return new NotFoundError($"Expense with id {request.Id} not found");

        var expense = _repository.GetExpenseById(request.Id);
        return _mapper.Map<ExpenseDto>(expense);
    });
}