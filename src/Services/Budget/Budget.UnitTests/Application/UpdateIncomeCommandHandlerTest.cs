using Budget.Application.Commands;
using Budget.Application.Commands.Handlers;
using Budget.Application.Dtos;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using MediatR;

namespace Budget.UnitTests.Application;

public class UpdateIncomeCommandHandlerTest
{
    private readonly Mock<IIncomeRepository> _repositoryMock;
    private readonly IIncomeRepository _repository;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IRequestHandler<UpdateIncomeCommand, Result> _handler;


    public UpdateIncomeCommandHandlerTest()
    {
        _repositoryMock = new Mock<IIncomeRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _repository = _repositoryMock.Object;

        var validator = new UpdateIncomeCommandValidator(_repository);

        _handler = new UpdateIncomeCommandHandler(_repository, _unitOfWorkMock.Object, validator);

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(It.IsAny<int>()))
            .Returns(true);

        _repositoryMock
            .Setup(x => x.GetIncomeById(It.IsAny<int>()))
            .Returns(GetDefaultIncome());
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess()
    {
        // Arrange
        var command = GetDefaultCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ShouldCommitUnitOfWork()
    {
        // Arrange
        var command = GetDefaultCommand();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCommandHasInvalidValues_ShouldReturnFailure()
    {
        // Arrange
        var command = GetCommandWithInvalidValues();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task Handle_WhenCommandHasInvalidValues_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var command = GetCommandWithInvalidValues();

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
            .Setup(x => x.ExistsIncomeWithId(command.Dto.Id))
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Single(result.Errors);
    }

    [Fact]
    public async Task Handle_WhenIncomeDoesNotExist_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(command.Dto.Id))
            .Returns(false);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenExistsAnotherIncomeWithDescriptionAndMonth_ShouldReturnFailureWithOneError()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsAnotherIncomeWithDescriptionAndMonth(command.Dto.Id, command.Dto.Description, command.Dto.Date.Month, command.Dto.Date.Year))
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Single(result.Errors);
    }

    [Fact]
    public async Task Handle_WhenExistsAnotherIncomeWithDescriptionAndMonth_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsAnotherIncomeWithDescriptionAndMonth(command.Dto.Id, command.Dto.Description, command.Dto.Date.Month, command.Dto.Date.Year))
            .Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static UpdateIncomeCommand GetDefaultCommand()
    {
        var (id, amount, description, date) = GetValidIncomeUpdateParameters();

        var dto = new IncomeDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date
        };

        return new UpdateIncomeCommand(dto);
    }

    private static UpdateIncomeCommand GetCommandWithInvalidValues()
    {
        var (_, _, _, date) = GetValidIncomeUpdateParameters();
        var (id, amount, description) = GetInvalidIncomeUpdateParameters();

        var dto = new IncomeDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date
        };

        return new UpdateIncomeCommand(dto);
    }

    private static (int, decimal, string, DateTime) GetValidIncomeUpdateParameters()
    {
        var id = 1;
        var amount = 200M;
        var description = "Test 2";
        var date = DateTime.UtcNow.AddMonths(1);

        return (id, amount, description, date);
    }

    private static (int, decimal, string) GetInvalidIncomeUpdateParameters()
    {
        var id = 0;
        var amount = 0M;
        var description = string.Empty;

        return (id, amount, description);
    }

    private Income GetDefaultIncome()
    {
        var amount = 100M;
        var description = "Test";
        var date = DateTime.UtcNow;

        return new Income(_repository, amount, description, date);
    }
}