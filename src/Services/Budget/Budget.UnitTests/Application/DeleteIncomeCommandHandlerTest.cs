using Budget.Application.Commands;
using Budget.Application.Commands.Handlers;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using MediatR;

namespace Budget.UnitTests.Application;

public class DeleteIncomeCommandHandlerTest
{
    private readonly Mock<IIncomeRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IRequestHandler<DeleteIncomeCommand, Result> _handler;

    public DeleteIncomeCommandHandlerTest()
    {
        _repositoryMock = new Mock<IIncomeRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var repository = _repositoryMock.Object;
        var validator = new DeleteIncomeCommandValidator(repository);

        _handler = new DeleteIncomeCommandHandler(repository, _unitOfWorkMock.Object, validator);

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(It.IsAny<int>()))
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
    public async Task Handle_WhenCommandHasInvalidId_ShouldNotDeleteIncome()
    {
        // Arrange
        var command = GetCommandWithInvalidId();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.DeleteIncome(It.IsAny<Income>()), Times.Never);
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
    public async Task Handle_WhenIncomeDoesNotExist_ShouldReturnFailureWithOneError()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(command.Id))
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Single(result.Errors);
    }

    [Fact]
    public async Task Handle_WhenIncomeDoesNotExist_ShouldNotDeleteIncome()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(command.Id))
            .Returns(false);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.DeleteIncome(It.IsAny<Income>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenIncomeDoesNotExist_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(command.Id))
            .Returns(false);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static DeleteIncomeCommand GetDefaultCommand()
    {
        return new DeleteIncomeCommand(1);
    }

    private static DeleteIncomeCommand GetCommandWithInvalidId()
    {
        return new DeleteIncomeCommand(0);
    }
}