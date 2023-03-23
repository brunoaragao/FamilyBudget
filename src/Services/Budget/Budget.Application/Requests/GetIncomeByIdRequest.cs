using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record GetIncomeByIdRequest(int Id) : IRequest<Result<IncomeDto>>;