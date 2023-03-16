using Budget.Application.Dtos;
using Budget.Application.Queries;
using Budget.Application.Queries.Handlers;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentResults;

using MediatR;

namespace Budget.UnitTests.Application;

public class GetExpensesQueryHandlerTest
{
    private readonly Mock<IExpenseRepository> _repositoryMock;
    private readonly IExpenseRepository _repository;
    private readonly IRequestHandler<GetExpensesQuery, Result<IEnumerable<ExpenseDto>>> _handler;

    public GetExpensesQueryHandlerTest()
    {
        _repositoryMock = new Mock<IExpenseRepository>();
        _repository = _repositoryMock.Object;

        _handler = new GetExpensesQueryHandler(_repository);

        _repositoryMock
            .Setup(r => r.GetExpenses())
            .Returns(GetDefaultExpenses());
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccessResultWithExpenses()
    {
        // Arrange
        var query = GetDefaultQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    private static GetExpensesQuery GetDefaultQuery()
    {
        return new GetExpensesQuery();
    }

    private IEnumerable<Expense> GetDefaultExpenses()
    {
        yield return new Expense(_repository, 100M, "Expense 1", DateTime.UtcNow, ExpenseCategory.Others);
        yield return new Expense(_repository, 200M, "Expense 2", DateTime.UtcNow, ExpenseCategory.Food);
    }
}