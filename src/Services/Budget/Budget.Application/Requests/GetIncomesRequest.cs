using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record GetIncomesRequest() : IRequest<Result<IEnumerable<IncomeDto>>>;