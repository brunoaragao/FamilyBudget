using AutoMapper;

using Budget.Application.Dtos;
using Budget.Application.Errors;
using Budget.Application.Handlers;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.SeedWork;

namespace Budget.UnitTests.Application;

public class UpdateExpenseRequestHandlerTest
{
    readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    readonly Mock<IExpenseRepository> _repositoryMock = new();
    readonly UpdateExpenseRequestHandler _handler;
    readonly UpdateExpenseRequest _defaultRequest = new(Dto: new() { Id = 1, Amount = 1M, Description = "Test", Date = default, Category = ExpenseCategoryDto.Others });

    public UpdateExpenseRequestHandlerTest()
    {
        var repository = _repositoryMock.Object;

        var fakeExpense = new Expense(repository, 1M, "Test", default, ExpenseCategory.Others.Id);

        var config = new MapperConfiguration(cfg => cfg.AddMaps("Budget.Application"));
        var mapper = config.CreateMapper();

        _handler = new UpdateExpenseRequestHandler(repository, _unitOfWorkMock.Object, mapper);

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(It.IsAny<int>()))
            .Returns(true);

        _repositoryMock
            .Setup(x => x.GetExpenseById(It.IsAny<int>()))
            .Returns(fakeExpense);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ShouldCommitUnitOfWork()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenExpenseDoesNotExist_ShouldReturnFailedResultWithNotFoundError()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(request.Dto.Id))
            .Returns(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.True(result.HasError<NotFoundError>());
    }

    [Fact]
    public async Task Handle_WhenExpenseDoesNotExist_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(request.Dto.Id))
            .Returns(false);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenExistsAnotherExpenseWithDescriptionAndMonth_ShouldReturnFailedResultWithConflictError()
    {
        // Arrange
        var request = _defaultRequest;
        var (id, amount, description, date, category) = request.Dto;

        _repositoryMock
            .Setup(x => x.ExistsAnotherExpenseWithDescriptionAndMonth(id, description, date.Month, date.Year))
            .Returns(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.True(result.HasError<ConflictError>());
    }

    [Fact]
    public async Task Handle_WhenExistsAnotherExpenseWithDescriptionAndMonth_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var request = _defaultRequest;
        var (id, amount, description, date, category) = request.Dto;

        _repositoryMock
            .Setup(x => x.ExistsAnotherExpenseWithDescriptionAndMonth(id, description, date.Month, date.Year))
            .Returns(true);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Never);
    }
}