using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record GetExpenseByIdRequest(int Id) : IRequest<Result<ExpenseDto>>;