using AutoMapper;

using Budget.Application.Queries.Handlers;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.ExpenseAggregates;

namespace Budget.Application.Handlers;

public class GetExpensesByMonthRequestHandlerTest
{
    readonly Mock<IExpenseRepository> _repositoryMock = new();
    readonly GetExpensesByMonthRequestHandler _handler;
    readonly GetExpensesByMonthRequest _defaultRequest = new(Month: 1, Year: 1);

    public GetExpensesByMonthRequestHandlerTest()
    {
        IExpenseRepository repository = _repositoryMock.Object;

        Expense[] fakeExpenses =
        {
            new(repository, 1M, "Test", default, ExpenseCategory.Others.Id),
            new(repository, 1M, "Test", default, ExpenseCategory.Others.Id)
        };

        _repositoryMock
            .Setup(r => r.GetExpensesByMonth(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(fakeExpenses);

        var config = new MapperConfiguration(cfg => cfg.AddMaps("Budget.Application"));
        var mapper = config.CreateMapper();

        _handler = new(repository, mapper);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccessResultWithValues()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Handle_WhenDoesNotExistsExpensesWithMonth_ShouldReturnSuccessResultWithEmptyValues()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(r => r.GetExpensesByMonth(request.Month, request.Year))
            .Returns(Enumerable.Empty<Expense>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }
}