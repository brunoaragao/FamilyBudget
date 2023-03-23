using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record DeleteIncomeRequest(int Id) : IRequest<Result<IncomeDto>>;