using Budget.Application.Dtos;

using FluentResults;

using MediatR;

namespace Budget.Application.Requests;

public record GetExpensesByDescriptionSnippetRequest(string DescriptionSnippet) : IRequest<Result<IEnumerable<ExpenseDto>>>;