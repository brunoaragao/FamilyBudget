using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Commands;

public record UpdateExpenseCommand(ExpenseDto Dto) : IRequest<Result>;