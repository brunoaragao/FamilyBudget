using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries;

public record GetExpenseByIdQuery(int Id) : IRequest<Result<ExpenseDto>>;