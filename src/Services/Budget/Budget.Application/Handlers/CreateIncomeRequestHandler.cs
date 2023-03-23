using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Errors;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using MediatR;

namespace Budget.Application.Handlers;

public class CreateIncomeRequestHandler : IRequestHandler<CreateIncomeRequest, Result<IncomeDto>>
{
    private readonly IIncomeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateIncomeRequestHandler(IIncomeRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Result<IncomeDto>> Handle(CreateIncomeRequest request, CancellationToken cancellationToken) => Task.Run<Result<IncomeDto>>(() =>
    {
        var (_, amount, description, date) = request.Dto;

        if (_repository.ExistsIncomeWithDescriptionAndMonth(description, date.Month, date.Year))
            return new ConflictError("Income with same description already exists for this month");

        var income = new Income(_repository, amount, description, date);

        _repository.AddIncome(income);
        _unitOfWork.Commit();

        return _mapper.Map<IncomeDto>(income);
    });
}