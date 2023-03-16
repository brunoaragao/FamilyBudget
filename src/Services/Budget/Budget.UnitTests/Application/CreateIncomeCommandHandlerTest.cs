using Budget.Application.Commands;
using Budget.Application.Commands.Handlers;
using Budget.Application.Dtos;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using MediatR;

namespace Budget.UnitTests.Application;

public class CreateIncomeCommandHandlerTest
{
    private readonly Mock<IIncomeRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IRequestHandler<CreateIncomeCommand, Result<int>> _handler;

    public CreateIncomeCommandHandlerTest()
    {
        _repositoryMock = new Mock<IIncomeRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var validator = new CreateIncomeCommandValidator(_repositoryMock.Object);

        _handler = new CreateIncomeCommandHandler(_repositoryMock.Object, _unitOfWorkMock.Object, validator);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnSuccess()
    {
        // Arrange
        var command = GetValidCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldAddIncomeToRepository()
    {
        // Arrange
        var command = GetValidCommand();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.AddIncome(It.IsAny<Income>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCommitUnitOfWork()
    {
        // Arrange
        var command = GetValidCommand();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCommandHasAnInvalidProperty_ShouldReturnFailure()
    {
        // Arrange
        var command = GetInvalidCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task Handle_WithInvalidCommand_ShouldNotAddIncomeToRepository()
    {
        // Arrange
        var command = GetInvalidCommand();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.AddIncome(It.IsAny<Income>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithInvalidCommand_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var command = GetInvalidCommand();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(CancellationToken.None), Times.Never);
    }

    private static CreateIncomeCommand GetValidCommand()
    {
        var (id, amount, description, date) = GetValidIncomeConstructorParameters();

        var dto = new IncomeDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date
        };

        return new CreateIncomeCommand(dto);
    }

    private static CreateIncomeCommand GetInvalidCommand()
    {
        var (_, _, _, date) = GetValidIncomeConstructorParameters();
        var (id, amount, description) = GetInvalidIncomeConstructorParameters();

        var dto = new IncomeDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date
        };

        return new CreateIncomeCommand(dto);
    }

    private static (int, decimal, string, DateTime) GetValidIncomeConstructorParameters()
    {
        var id = 0;
        var amount = 100m;
        var description = "Test";
        var date = DateTime.UtcNow;

        return (id, amount, description, date);
    }

    private static (int, decimal, string) GetInvalidIncomeConstructorParameters()
    {
        var id = 1;
        var amount = 0m;
        var description = string.Empty;

        return (id, amount, description);
    }
}