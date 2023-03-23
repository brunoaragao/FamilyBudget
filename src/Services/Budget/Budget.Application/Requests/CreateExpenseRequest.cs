using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record CreateExpenseRequest(ExpenseDto Dto) : IRequest<Result<ExpenseDto>>;