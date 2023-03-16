using Budget.Application.Dtos;
using Budget.Application.Queries;
using Budget.Application.Queries.Handlers;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentResults;

using MediatR;

namespace Budget.Application.Commands.Handlers;

public class GetIncomesByMonthQueryHandlerTest
{
    private readonly Mock<IIncomeRepository> _repositoryMock;
    private readonly IIncomeRepository _repository;
    private readonly IRequestHandler<GetIncomesByMonthQuery, Result<IEnumerable<IncomeDto>>> _handler;

    public GetIncomesByMonthQueryHandlerTest()
    {
        _repositoryMock = new Mock<IIncomeRepository>();
        _repository = _repositoryMock.Object;

        _repositoryMock
            .Setup(r => r.GetIncomesByMonth(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(GetDefaultIncomes());

        var validator = new GetIncomesByMonthQueryValidator();

        _handler = new GetIncomesByMonthQueryHandler(_repository, validator);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccessResultWithValues()
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
    public async Task Handle_WhenDoesNotExistsIncomesWithMonth_ShouldReturnSuccessResultWithEmptyValues()
    {
        // Arrange
        var query = GetDefaultQuery();

        _repositoryMock
            .Setup(r => r.GetIncomesByMonth(query.Month, query.Year))
            .Returns(Enumerable.Empty<Income>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task Handle_WhenQueryHasInvalidValues_ShouldReturnFailedResult()
    {
        // Arrange
        var query = GetQueryWithInvalidValues();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
    }

    private static GetIncomesByMonthQuery GetDefaultQuery()
    {
        return new GetIncomesByMonthQuery(1, 0001);
    }

    private static GetIncomesByMonthQuery GetQueryWithInvalidValues()
    {
        return new GetIncomesByMonthQuery(0, 0000);
    }

    private IEnumerable<Income> GetDefaultIncomes()
    {
        yield return new Income(_repository, 100M, "Income 1", DateTime.UtcNow);
        yield return new Income(_repository, 200M, "Income 2", DateTime.UtcNow);
    }
}