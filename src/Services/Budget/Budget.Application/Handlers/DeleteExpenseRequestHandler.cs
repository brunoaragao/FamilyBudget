using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Errors;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using MediatR;

namespace Budget.Application.Handlers;

public class DeleteExpenseRequestHandler : IRequestHandler<DeleteExpenseRequest, Result<ExpenseDto>>
{
    private readonly IExpenseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteExpenseRequestHandler(IExpenseRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Result<ExpenseDto>> Handle(DeleteExpenseRequest request, CancellationToken cancellationToken) => Task.Run<Result<ExpenseDto>>(() =>
    {
        var id = request.Id;

        if (!_repository.ExistsExpenseWithId(id))
            return new NotFoundError($"Expense with id {id} not found");

        var expense = _repository.GetExpenseById(id);

        _repository.DeleteExpense(expense);
        _unitOfWork.Commit();

        return _mapper.Map<ExpenseDto>(expense);
    });
}