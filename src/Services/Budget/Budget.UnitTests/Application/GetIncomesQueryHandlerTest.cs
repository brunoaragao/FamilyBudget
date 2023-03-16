using Budget.Application.Dtos;
using Budget.Application.Queries;
using Budget.Application.Queries.Handlers;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentResults;

using MediatR;

namespace Budget.UnitTests.Application;

public class GetIncomesQueryHandlerTest
{
    private readonly Mock<IIncomeRepository> _repositoryMock;
    private readonly IIncomeRepository _repository;
    private readonly IRequestHandler<GetIncomesQuery, Result<IEnumerable<IncomeDto>>> _handler;

    public GetIncomesQueryHandlerTest()
    {
        _repositoryMock = new Mock<IIncomeRepository>();
        _repository = _repositoryMock.Object;

        _handler = new GetIncomesQueryHandler(_repository);

        _repositoryMock
            .Setup(r => r.GetIncomes())
            .Returns(GetDefaultIncomes());
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccessResultWithIncomes()
    {
        // Arrange
        var query = GetDefaultQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    private static GetIncomesQuery GetDefaultQuery()
    {
        return new GetIncomesQuery();
    }

    private IEnumerable<Income> GetDefaultIncomes()
    {
        yield return new Income(_repository, 100M, "Income 1", DateTime.UtcNow);
        yield return new Income(_repository, 200M, "Income 2", DateTime.UtcNow);
    }
}