using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record GetMonthlySummaryRequest(int Month, int Year) : IRequest<Result<MonthlyBudgetSummaryDto>>;