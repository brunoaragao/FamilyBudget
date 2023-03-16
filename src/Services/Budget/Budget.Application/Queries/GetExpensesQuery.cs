using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries;

public record GetExpensesQuery() : IRequest<Result<IEnumerable<ExpenseDto>>>;