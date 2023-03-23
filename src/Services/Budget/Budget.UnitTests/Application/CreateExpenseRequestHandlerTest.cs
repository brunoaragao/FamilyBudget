using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Errors;
using Budget.Application.Handlers;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.SeedWork;

namespace Budget.UnitTests.Application;

public class CreateExpenseRequestHandlerTest
{
    readonly Mock<IExpenseRepository> _repositoryMock = new();
    readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    readonly CreateExpenseRequestHandler _handler;
    readonly CreateExpenseRequest _defaultRequest = new(Dto: new() { Id = 0, Amount = 1M, Description = "Test", Date = default, Category = ExpenseCategoryDto.Others });

    public CreateExpenseRequestHandlerTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps("Budget.Application"));
        var mapper = config.CreateMapper();

        _handler = new(_repositoryMock.Object, _unitOfWorkMock.Object, mapper);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccessResult()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldAddExpenseToRepository()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.AddExpense(It.IsAny<Expense>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldCommitUnitOfWork()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenExpenseWithSameDescriptionAndMonthAlreadyExists_ShouldReturnFailedResultWithConflictError()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithDescriptionAndMonth(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.True(result.HasError<ConflictError>());
    }

    [Fact]
    public async Task Handle_WhenExpenseWithSameDescriptionAndMonthAlreadyExists_ShouldNotAddExpenseToRepository()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithDescriptionAndMonth(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.AddExpense(It.IsAny<Expense>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenExpenseWithSameDescriptionAndMonthAlreadyExists_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithDescriptionAndMonth(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Never);
    }
}