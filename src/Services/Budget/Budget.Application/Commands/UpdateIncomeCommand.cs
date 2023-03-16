using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Commands;

public record UpdateIncomeCommand(IncomeDto Dto) : IRequest<Result>;