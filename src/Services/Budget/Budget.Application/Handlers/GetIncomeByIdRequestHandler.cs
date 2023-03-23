using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Errors;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries.Handlers;

public class GetIncomeByIdRequestHandler : IRequestHandler<GetIncomeByIdRequest, Result<IncomeDto>>
{
    private readonly IIncomeRepository _repository;
    private readonly IMapper _mapper;

    public GetIncomeByIdRequestHandler(IIncomeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<Result<IncomeDto>> Handle(GetIncomeByIdRequest request, CancellationToken cancellationToken) => Task.Run<Result<IncomeDto>>(() =>
    {
        if (!_repository.ExistsIncomeWithId(request.Id))
            return new NotFoundError($"Income with id {request.Id} not found");

        var income = _repository.GetIncomeById(request.Id);
        return _mapper.Map<IncomeDto>(income);
    });
}