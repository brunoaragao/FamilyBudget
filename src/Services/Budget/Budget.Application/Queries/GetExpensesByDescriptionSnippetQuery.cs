using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Queries;

public record GetExpensesByDescriptionSnippetQuery(string DescriptionSnippet) : IRequest<Result<IEnumerable<ExpenseDto>>>;