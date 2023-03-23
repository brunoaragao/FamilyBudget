using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Errors;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using MediatR;

namespace Budget.Application.Handlers;

public class CreateExpenseRequestHandler : IRequestHandler<CreateExpenseRequest, Result<ExpenseDto>>
{
    private readonly IExpenseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateExpenseRequestHandler(IExpenseRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Result<ExpenseDto>> Handle(CreateExpenseRequest request, CancellationToken cancellationToken) => Task.Run<Result<ExpenseDto>>(() =>
    {
        var (_, amount, description, date, categoryDto) = request.Dto;

        if (_repository.ExistsExpenseWithDescriptionAndMonth(description, date.Month, date.Year))
            return new ConflictError("Expense with same description already exists for this month");

        var expense = new Expense(_repository, amount, description, date, (int)categoryDto);

        _repository.AddExpense(expense);
        _unitOfWork.Commit();

        var expenseDto = _mapper.Map<ExpenseDto>(expense);
        return expenseDto;
    });
}