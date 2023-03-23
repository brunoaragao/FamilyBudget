using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Errors;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using MediatR;

namespace Budget.Application.Handlers;

public class UpdateIncomeRequestHandler : IRequestHandler<UpdateIncomeRequest, Result<IncomeDto>>
{
    private readonly IIncomeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateIncomeRequestHandler(IIncomeRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IncomeDto>> Handle(UpdateIncomeRequest request, CancellationToken cancellationToken) => await Task.Run<Result<IncomeDto>>(() =>
    {
        var (id, amount, description, date) = request.Dto;

        if (!_repository.ExistsIncomeWithId(id))
            return new NotFoundError($"Income with id {id} not found");

        if (_repository.ExistsAnotherIncomeWithDescriptionAndMonth(id, description, date.Month, date.Year))
            return new ConflictError("Income with same description already exists for this month");

        var income = _repository.GetIncomeById(id);

        income.Update(_repository, amount, description, date);
        _unitOfWork.Commit();

        return _mapper.Map<IncomeDto>(income);
    });
}