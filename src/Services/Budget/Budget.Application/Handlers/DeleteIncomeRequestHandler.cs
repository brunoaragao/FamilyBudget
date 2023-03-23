using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Errors;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using MediatR;

namespace Budget.Application.Handlers;

public class DeleteIncomeRequestHandler : IRequestHandler<DeleteIncomeRequest, Result<IncomeDto>>
{
    private readonly IIncomeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteIncomeRequestHandler(IIncomeRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Result<IncomeDto>> Handle(DeleteIncomeRequest request, CancellationToken cancellationToken) => Task.Run<Result<IncomeDto>>(() =>
    {
        var id = request.Id;

        if (!_repository.ExistsIncomeWithId(id))
            return new NotFoundError($"Income with id {id} not found");

        var income = _repository.GetIncomeById(id);

        _repository.DeleteIncome(income);
        _unitOfWork.Commit();

        return _mapper.Map<IncomeDto>(income);
    });
}