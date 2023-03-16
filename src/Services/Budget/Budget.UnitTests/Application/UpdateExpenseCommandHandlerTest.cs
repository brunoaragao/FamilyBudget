using Budget.Application.Commands;
using Budget.Application.Commands.Handlers;
using Budget.Application.Dtos;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using MediatR;

namespace Budget.UnitTests.Application;

public class UpdateExpenseCommandHandlerTest
{
    private readonly Mock<IExpenseRepository> _repositoryMock;
    private readonly IExpenseRepository _repository;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IRequestHandler<UpdateExpenseCommand, Result> _handler;


    public UpdateExpenseCommandHandlerTest()
    {
        _repositoryMock = new Mock<IExpenseRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _repository = _repositoryMock.Object;

        var validator = new UpdateExpenseCommandValidator(_repository);

        _handler = new UpdateExpenseCommandHandler(_repository, _unitOfWorkMock.Object, validator);

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(It.IsAny<int>()))
            .Returns(true);

        _repositoryMock
            .Setup(x => x.GetExpenseById(It.IsAny<int>()))
            .Returns(GetDefaultExpense());
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
    public async Task Handle_WhenExpenseDoesNotExist_ShouldReturnFailureWithOneError()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(command.Dto.Id))
            .Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Single(result.Errors);
    }

    [Fact]
    public async Task Handle_WhenExpenseDoesNotExist_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsExpenseWithId(command.Dto.Id))
            .Returns(false);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenExistsAnotherExpenseWithDescriptionAndMonth_ShouldReturnFailureWithOneError()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsAnotherExpenseWithDescriptionAndMonth(command.Dto.Id, command.Dto.Description, command.Dto.Date.Month, command.Dto.Date.Year))
            .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Single(result.Errors);
    }

    [Fact]
    public async Task Handle_WhenExistsAnotherExpenseWithDescriptionAndMonth_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var command = GetDefaultCommand();

        _repositoryMock
            .Setup(x => x.ExistsAnotherExpenseWithDescriptionAndMonth(command.Dto.Id, command.Dto.Description, command.Dto.Date.Month, command.Dto.Date.Year))
            .Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private static UpdateExpenseCommand GetDefaultCommand()
    {
        var (id, amount, description, date, expenseCategory) = GetValidExpenseUpdateParameters();

        var dto = new ExpenseDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date,
            ExpenseCategory = expenseCategory
        };

        return new UpdateExpenseCommand(dto);
    }

    private static UpdateExpenseCommand GetCommandWithInvalidValues()
    {
        var (_, _, _, date, _) = GetValidExpenseUpdateParameters();
        var (id, amount, description, expenseCategory) = GetInvalidExpenseUpdateParameters();

        var dto = new ExpenseDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date,
            ExpenseCategory = expenseCategory
        };

        return new UpdateExpenseCommand(dto);
    }

    private static (int, decimal, string, DateTime, ExpenseCategoryDto) GetValidExpenseUpdateParameters()
    {
        var id = 1;
        var amount = 200M;
        var description = "Test 2";
        var date = DateTime.UtcNow.AddMonths(1);
        var expenseCategory = ExpenseCategoryDto.Food;

        return (id, amount, description, date, expenseCategory);
    }

    private static (int, decimal, string, ExpenseCategoryDto) GetInvalidExpenseUpdateParameters()
    {
        var id = 0;
        var amount = 0M;
        var description = string.Empty;
        var expenseCategory = (ExpenseCategoryDto)99;

        return (id, amount, description, expenseCategory);
    }

    private Expense GetDefaultExpense()
    {
        var amount = 100M;
        var description = "Test";
        var date = DateTime.UtcNow;
        var categoryDto = ExpenseCategoryDto.Others;
        var category = Enumeration.FromValue<ExpenseCategory>((int)categoryDto);

        return new Expense(_repository, amount, description, date, category);
    }
}