using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record UpdateExpenseRequest(ExpenseDto Dto) : IRequest<Result<ExpenseDto>>;