using FluentResults;

using MediatR;

namespace Budget.Application.Commands;

public record DeleteExpenseCommand(int Id) : IRequest<Result>;