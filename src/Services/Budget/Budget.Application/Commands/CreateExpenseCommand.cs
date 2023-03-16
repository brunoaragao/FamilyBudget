using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Commands;

public record CreateExpenseCommand(ExpenseDto Dto) : IRequest<Result<int>>;