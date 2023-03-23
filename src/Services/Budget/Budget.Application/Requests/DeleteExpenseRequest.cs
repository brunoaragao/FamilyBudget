using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record DeleteExpenseRequest(int Id) : IRequest<Result<ExpenseDto>>;