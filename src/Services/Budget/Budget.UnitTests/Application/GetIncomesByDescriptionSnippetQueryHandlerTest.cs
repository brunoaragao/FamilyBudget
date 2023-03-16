using Budget.Application.Dtos;
using Budget.Application.Queries;
using Budget.Application.Queries.Handlers;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentResults;

using MediatR;

namespace Budget.Application.Commands.Handlers;

public class GetIncomesByDescriptionSnippetQueryHandlerTest
{
    private readonly Mock<IIncomeRepository> _repositoryMock;
    private readonly IIncomeRepository _repository;
    private readonly IRequestHandler<GetIncomesByDescriptionSnippetQuery, Result<IEnumerable<IncomeDto>>> _handler;

    public GetIncomesByDescriptionSnippetQueryHandlerTest()
    {
        _repositoryMock = new Mock<IIncomeRepository>();
        _repository = _repositoryMock.Object;

        _repositoryMock
            .Setup(r => r.GetIncomesByDescriptionSnippet(It.IsAny<string>()))
            .Returns(GetDefaultIncomes());

        var validator = new GetIncomesByDescriptionSnippetQueryValidator();

        _handler = new GetIncomesByDescriptionSnippetQueryHandler(_repository, validator);
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
    public async Task Handle_WhenDoesNotExistsIncomesWithDescriptionSnippet_ShouldReturnSuccessResultWithEmptyValues()
    {
        // Arrange
        var query = GetDefaultQuery();

        _repositoryMock
            .Setup(r => r.GetIncomesByDescriptionSnippet(It.IsAny<string>()))
            .Returns(Enumerable.Empty<Income>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task Handle_WhenDescriptionSnippetValueIsInvalid_ShouldReturnFailureWithOneError()
    {
        // Arrange
        var query = GetQueryWithInvalidDescriptionSnippetValue();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Single(result.Errors);
    }

    private static GetIncomesByDescriptionSnippetQuery GetDefaultQuery()
    {
        return new GetIncomesByDescriptionSnippetQuery("Income");
    }

    private static GetIncomesByDescriptionSnippetQuery GetQueryWithInvalidDescriptionSnippetValue()
    {
        return new GetIncomesByDescriptionSnippetQuery(string.Empty);
    }

    private IEnumerable<Income> GetDefaultIncomes()
    {
        yield return new Income(_repository, 100M, "Income 1", DateTime.UtcNow);
        yield return new Income(_repository, 200M, "Income 2", DateTime.UtcNow);
    }
}