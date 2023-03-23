using AutoMapper;

using Budget.Application.Errors;
using Budget.Application.Queries.Handlers;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.ExpenseAggregates;

namespace Budget.UnitTests.Application;

public class GetExpenseByIdRequestHandlerTest
{
    readonly Mock<IExpenseRepository> _repositoryMock = new();
    readonly GetExpenseByIdRequestHandler _handler;
    readonly GetExpenseByIdRequest _defaultRequest = new(Id: 1);

    public GetExpenseByIdRequestHandlerTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps("Budget.Application"));
        var mapper = config.CreateMapper();

        var repository = _repositoryMock.Object;

        _handler = new(repository, mapper);

        var defaultExpense = new Expense(repository, 100, "Test", DateTime.UtcNow, ExpenseCategory.Others.Id);

        _repositoryMock
            .Setup(r => r.GetExpenseById(It.IsAny<int>()))
            .Returns(defaultExpense);

        _repositoryMock
            .Setup(r => r.ExistsExpenseWithId(It.IsAny<int>()))
            .Returns(true);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccessResultWithExpense()
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
    public async Task Handle_WhenExpenseDoesNotExist_ShouldReturnFailedResultWithNotFoundError()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(r => r.ExistsExpenseWithId(request.Id))
            .Returns(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.True(result.HasError<NotFoundError>());
    }
}