using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record GetIncomesByDescriptionSnippetRequest(string DescriptionSnippet) : IRequest<Result<IEnumerable<IncomeDto>>>;