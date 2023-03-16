using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries;

public record GetIncomeByIdQuery(int Id) : IRequest<Result<IncomeDto>>;