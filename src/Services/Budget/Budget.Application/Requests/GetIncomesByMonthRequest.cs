using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record GetIncomesByMonthRequest(int Month, int Year) : IRequest<Result<IEnumerable<IncomeDto>>>;