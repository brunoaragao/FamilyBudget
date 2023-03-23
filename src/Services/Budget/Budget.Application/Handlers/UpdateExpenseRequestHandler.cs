using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Errors;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using MediatR;

namespace Budget.Application.Handlers;

public class UpdateExpenseRequestHandler : IRequestHandler<UpdateExpenseRequest, Result<ExpenseDto>>
{
    private readonly IExpenseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateExpenseRequestHandler(IExpenseRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<ExpenseDto>> Handle(UpdateExpenseRequest request, CancellationToken cancellationToken) => await Task.Run<Result<ExpenseDto>>(() =>
    {
        var (id, amount, description, date, categoryDto) = request.Dto;

        if (!_repository.ExistsExpenseWithId(id))
            return new NotFoundError($"Expense with id {id} not found");

        if (_repository.ExistsAnotherExpenseWithDescriptionAndMonth(id, description, date.Month, date.Year))
            return new ConflictError("Another expense with same description already exists for this month");

        var expense = _repository.GetExpenseById(id);

        expense.Update(_repository, amount, description, date, (int)categoryDto);
        _unitOfWork.Commit();

        return _mapper.Map<ExpenseDto>(expense);
    });
}