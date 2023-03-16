using Budget.Application.Commands;
using Budget.Application.Commands.Handlers;
using Budget.Application.Dtos;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.ExpenseAggregates;
using Budget.Domain.SeedWork;

using FluentResults;

using MediatR;

namespace Budget.UnitTests.Application;

public class CreateExpenseCommandHandlerTest
{
    private readonly Mock<IExpenseRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly IRequestHandler<CreateExpenseCommand, Result<int>> _handler;

    public CreateExpenseCommandHandlerTest()
    {
        _repositoryMock = new Mock<IExpenseRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        var validator = new CreateExpenseCommandValidator(_repositoryMock.Object);

        _handler = new CreateExpenseCommandHandler(_repositoryMock.Object, _unitOfWorkMock.Object, validator);
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
    public async Task Handle_ShouldAddExpenseToRepository()
    {
        // Arrange
        var command = GetDefaultCommand();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.AddExpense(It.IsAny<Expense>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldCommitUnitOfWork()
    {
        // Arrange
        var command = GetDefaultCommand();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(CancellationToken.None), Times.Once);
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
    public async Task Handle_WhenCommandHasInvalidValues_ShouldNotAddExpenseToRepository()
    {
        // Arrange
        var command = GetCommandWithInvalidValues();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.AddExpense(It.IsAny<Expense>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenCommandHasInvalidValues_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var command = GetCommandWithInvalidValues();

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.CommitAsync(CancellationToken.None), Times.Never);
    }

    private static CreateExpenseCommand GetDefaultCommand()
    {
        var (id, amount, description, date, expenseCategory) = GetValidExpenseConstructorParameters();

        var dto = new ExpenseDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date,
            ExpenseCategory = expenseCategory
        };

        return new CreateExpenseCommand(dto);
    }

    private static CreateExpenseCommand GetCommandWithInvalidValues()
    {
        var (_, _, _, date, _) = GetValidExpenseConstructorParameters();
        var (id, amount, description, expenseCategory) = GetInvalidExpenseConstructorParameters();

        var dto = new ExpenseDto
        {
            Id = id,
            Amount = amount,
            Description = description,
            Date = date,
            ExpenseCategory = expenseCategory
        };

        return new CreateExpenseCommand(dto);
    }

    private static (int, decimal, string, DateTime, ExpenseCategoryDto) GetValidExpenseConstructorParameters()
    {
        var id = 0;
        var amount = 100m;
        var description = "Test";
        var date = DateTime.UtcNow;
        var expenseCategory = ExpenseCategoryDto.Others;

        return (id, amount, description, date, expenseCategory);
    }

    private static (int, decimal, string, ExpenseCategoryDto) GetInvalidExpenseConstructorParameters()
    {
        var id = 1;
        var amount = 0m;
        var description = string.Empty;
        var expenseCategory = (ExpenseCategoryDto)99;

        return (id, amount, description, expenseCategory);
    }
}