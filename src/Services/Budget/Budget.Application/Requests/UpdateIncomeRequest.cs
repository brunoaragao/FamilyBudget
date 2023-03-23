using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record UpdateIncomeRequest(IncomeDto Dto) : IRequest<Result<IncomeDto>>;