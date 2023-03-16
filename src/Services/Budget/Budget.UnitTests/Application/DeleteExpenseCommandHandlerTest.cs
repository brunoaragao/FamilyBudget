using Budget.Application.Commands;
using Budget.Application.Commands.Handlers;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using MediatR;

namespace Budget.UnitTests.Application;

public class DeleteExpenseCommandHandlerTest
{
    private readonly Mock<IExpenseRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IRequestHandler<DeleteExpenseCommand, Result> _handler;

    public DeleteExpenseCommandHandlerTest()
    {
        _repositoryMock = new Mock<IExpenseRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var repository = _repositoryMock.Object;
        var validator = new DeleteExpenseCommandValidator(repository);

        _handler = new DeleteExpenseCommandHandler(repository, _unitOfWorkMock.Object, validator);

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(It.IsAny<int>()))
            .Returns(true);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccess()
    {
        // Arrange
        var command = GetDefaultCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldCommitUnitOfWork()
    {
        // Arrange
        var command = GetDefaultCommand();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCommandHasInvalidId_ShouldReturnFailureWithOneError()
    {
        // Arrange
        var command = GetCommandWithInvalidId();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Single(result.Errors);
    }

    [Fact]
    public async Task Handle_WhenCommandHasInvalidId_ShouldNotDeleteExpense()
    {
        // Arrange
        var command = GetCommandWithInvalidId();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.DeleteExpense(It.IsAny<Expense>()), Times.Never);
    }


    [Fact]
    public async Task Handle_WhenCommandHasInvalidId_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var command = GetCommandWithInvalidId();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenExpenseDoesNotExist_ShouldReturnFailureWithOneError()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(command.Id))
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Single(result.Errors);
    }

    [Fact]
    public async Task Handle_WhenExpenseDoesNotExist_ShouldNotDeleteExpense()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(command.Id))
            .Returns(false);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.DeleteExpense(It.IsAny<Expense>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenExpenseDoesNotExist_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(command.Id))
            .Returns(false);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static DeleteExpenseCommand GetDefaultCommand()
    {
        return new DeleteExpenseCommand(1);
    }

    private static DeleteExpenseCommand GetCommandWithInvalidId()
    {
        return new DeleteExpenseCommand(0);
    }
}