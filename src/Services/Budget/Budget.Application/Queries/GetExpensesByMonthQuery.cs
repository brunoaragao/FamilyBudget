using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries;

public record GetExpensesByMonthQuery(int Month, int Year) : IRequest<Result<IEnumerable<ExpenseDto>>>;