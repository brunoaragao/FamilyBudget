using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetExpensesByDescriptionSnippetRequestHandler : IRequestHandler<GetExpensesByDescriptionSnippetRequest, Result<IEnumerable<ExpenseDto>>>
{
    private readonly IExpenseRepository _repository;
    private readonly IMapper _mapper;

    public GetExpensesByDescriptionSnippetRequestHandler(IExpenseRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<Result<IEnumerable<ExpenseDto>>> Handle(GetExpensesByDescriptionSnippetRequest request, CancellationToken cancellationToken) => Task.Run<Result<IEnumerable<ExpenseDto>>>(() =>
    {
        var expenses = _repository.GetExpensesByDescriptionSnippet(request.DescriptionSnippet);
        var dtos = _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
        return Result.Ok(dtos);
    });
}