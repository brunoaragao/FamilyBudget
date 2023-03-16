using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries;

public record GetIncomesQuery() : IRequest<Result<IEnumerable<IncomeDto>>>;