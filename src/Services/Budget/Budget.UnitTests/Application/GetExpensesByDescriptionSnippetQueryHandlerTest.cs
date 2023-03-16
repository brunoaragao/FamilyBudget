using Budget.Application.Dtos;
using Budget.Application.Queries;
using Budget.Application.Queries.Handlers;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentResults;

using MediatR;

namespace Budget.Application.Commands.Handlers;

public class GetExpensesByDescriptionSnippetQueryHandlerTest
{
    private readonly Mock<IExpenseRepository> _repositoryMock;
    private readonly IExpenseRepository _repository;
    private readonly IRequestHandler<GetExpensesByDescriptionSnippetQuery, Result<IEnumerable<ExpenseDto>>> _handler;

    public GetExpensesByDescriptionSnippetQueryHandlerTest()
    {
        _repositoryMock = new Mock<IExpenseRepository>();
        _repository = _repositoryMock.Object;

        _repositoryMock
            .Setup(r => r.GetExpensesByDescriptionSnippet(It.IsAny<string>()))
            .Returns(GetDefaultExpenses());

        var validator = new GetExpensesByDescriptionSnippetQueryValidator();

        _handler = new GetExpensesByDescriptionSnippetQueryHandler(_repository, validator);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccessResultWithValues()
    {
        // Arrange
        var query = GetDefaultQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Handle_WhenDoesNotExistsExpensesWithDescriptionSnippet_ShouldReturnSuccessResultWithEmptyValues()
    {
        // Arrange
        var query = GetDefaultQuery();

        _repositoryMock
            .Setup(r => r.GetExpensesByDescriptionSnippet(query.DescriptionSnippet))
            .Returns(Enumerable.Empty<Expense>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task Handle_WhenDescriptionSnippetValueIsInvalid_ShouldReturnFailureWithOneError()
    {
        // Arrange
        var query = GetQueryWithInvalidDescriptionSnippetValue();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Single(result.Errors);
    }

    private static GetExpensesByDescriptionSnippetQuery GetDefaultQuery()
    {
        return new GetExpensesByDescriptionSnippetQuery("Expense");
    }

    private static GetExpensesByDescriptionSnippetQuery GetQueryWithInvalidDescriptionSnippetValue()
    {
        return new GetExpensesByDescriptionSnippetQuery(string.Empty);
    }

    private IEnumerable<Expense> GetDefaultExpenses()
    {
        yield return new Expense(_repository, 100M, "Expense 1", DateTime.UtcNow, ExpenseCategory.Others);
        yield return new Expense(_repository, 200M, "Expense 2", DateTime.UtcNow, ExpenseCategory.Food);
    }
}