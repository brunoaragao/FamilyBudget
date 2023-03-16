using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries;

public record GetIncomesByDescriptionSnippetQuery(string DescriptionSnippet) : IRequest<Result<IEnumerable<IncomeDto>>>;