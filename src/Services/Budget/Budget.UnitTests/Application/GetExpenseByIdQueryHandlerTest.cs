using Budget.Application.Dtos;
using Budget.Application.Queries;
using Budget.Application.Queries.Handlers;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.ExpenseAggregates;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.UnitTests.Application;

public class GetExpenseByIdQueryHandlerTest
{
    private readonly Mock<IExpenseRepository> _repositoryMock;
    private readonly IExpenseRepository _repository;
    private readonly IValidator<GetExpenseByIdQuery> _validator;
    private readonly IRequestHandler<GetExpenseByIdQuery, Result<ExpenseDto>> _handler;

    public GetExpenseByIdQueryHandlerTest()
    {
        _repositoryMock = new Mock<IExpenseRepository>();
        _repository = _repositoryMock.Object;

        _validator = new GetExpenseByIdQueryValidator(_repository);

        _handler = new GetExpenseByIdQueryHandler(_repository, _validator);

        _repositoryMock
            .Setup(r => r.GetExpenseById(It.IsAny<int>()))
            .Returns(GetDefaultExpense());

        _repositoryMock
            .Setup(r => r.ExistsExpenseWithId(It.IsAny<int>()))
            .Returns(true);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccessResultWithExpense()
    {
        // Arrange
        var query = GetDefaultQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Handle_WhenExpenseDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var query = GetDefaultQuery();

        _repositoryMock
            .Setup(r => r.ExistsExpenseWithId(query.Id))
            .Returns(false);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
    }

    private static GetExpenseByIdQuery GetDefaultQuery()
    {
        return new GetExpenseByIdQuery(1);
    }

    private Expense GetDefaultExpense()
    {
        return new Expense(_repository, 100, "Test", DateTime.UtcNow, ExpenseCategory.Others);
    }
}