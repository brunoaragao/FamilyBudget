using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record GetExpensesRequest() : IRequest<Result<IEnumerable<ExpenseDto>>>;