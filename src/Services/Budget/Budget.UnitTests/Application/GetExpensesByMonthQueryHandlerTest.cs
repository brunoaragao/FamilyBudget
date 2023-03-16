using Budget.Application.Dtos;
using Budget.Application.Queries;
using Budget.Application.Queries.Handlers;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentResults;

using MediatR;

namespace Budget.Application.Commands.Handlers;

public class GetExpensesByMonthQueryHandlerTest
{
    private readonly Mock<IExpenseRepository> _repositoryMock;
    private readonly IExpenseRepository _repository;
    private readonly IRequestHandler<GetExpensesByMonthQuery, Result<IEnumerable<ExpenseDto>>> _handler;

    public GetExpensesByMonthQueryHandlerTest()
    {
        _repositoryMock = new Mock<IExpenseRepository>();
        _repository = _repositoryMock.Object;

        _repositoryMock
            .Setup(r => r.GetExpensesByMonth(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(GetDefaultExpenses());

        var validator = new GetExpensesByMonthQueryValidator();

        _handler = new GetExpensesByMonthQueryHandler(_repository, validator);
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
    public async Task Handle_WhenDoesNotExistsExpensesWithMonth_ShouldReturnSuccessResultWithEmptyValues()
    {
        // Arrange
        var query = GetDefaultQuery();

        _repositoryMock
            .Setup(r => r.GetExpensesByMonth(query.Month, query.Year))
            .Returns(Enumerable.Empty<Expense>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task Handle_WhenQueryHasInvalidValues_ShouldReturnFailedResult()
    {
        // Arrange
        var query = GetQueryWithInvalidValues();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
    }

    private static GetExpensesByMonthQuery GetDefaultQuery()
    {
        return new GetExpensesByMonthQuery(1, 0001);
    }

    private static GetExpensesByMonthQuery GetQueryWithInvalidValues()
    {
        return new GetExpensesByMonthQuery(0, 0000);
    }

    private IEnumerable<Expense> GetDefaultExpenses()
    {
        yield return new Expense(_repository, 100M, "Expense 1", DateTime.UtcNow, ExpenseCategory.Others);
        yield return new Expense(_repository, 200M, "Expense 2", DateTime.UtcNow, ExpenseCategory.Food);
    }
}