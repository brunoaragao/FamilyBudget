using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries;

public record GetIncomesByMonthQuery(int Month, int Year) : IRequest<Result<IEnumerable<IncomeDto>>>;