using FluentResults;

using MediatR;

namespace Budget.Application.Commands;

public record DeleteIncomeCommand(int Id) : IRequest<Result>;