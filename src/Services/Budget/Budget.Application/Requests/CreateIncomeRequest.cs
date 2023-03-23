using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record CreateIncomeRequest(IncomeDto Dto) : IRequest<Result<IncomeDto>>;