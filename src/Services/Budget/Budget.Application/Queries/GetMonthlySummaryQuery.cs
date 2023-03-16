using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries;

public record GetMonthlySummaryQuery(int Month, int Year) : IRequest<Result<MonthlyBudgetSummaryDto>>;