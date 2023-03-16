using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Commands;

public record CreateIncomeCommand(IncomeDto Dto) : IRequest<Result<int>>;