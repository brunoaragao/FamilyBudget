using AutoMapper;

using Budget.Application.Queries.Handlers;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.IncomeAggregates;

namespace Budget.UnitTests.Application;

public class GetIncomesRequestHandlerTest
{
    readonly Mock<IIncomeRepository> _repositoryMock = new();
    readonly GetIncomesRequestHandler _handler;

    readonly GetIncomesRequest _defaultRequest = new();

    public GetIncomesRequestHandlerTest()
    {
        var repository = _repositoryMock.Object;

        var config = new MapperConfiguration(cfg => cfg.AddMaps("Budget.Application"));
        var mapper = config.CreateMapper();

        _handler = new(repository, mapper);

        Income[] fakeIncomes =
        {
            new(repository, 1M, "Test", default),
            new(repository, 1M, "Test", default)
        };

        _repositoryMock
            .Setup(r => r.GetIncomes())
            .Returns(fakeIncomes);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccessResultWithIncomes()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Handle_WhenDoesNotExistsIncomes_ShouldReturnSuccessResultWithEmptyIncomes()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(r => r.GetIncomes())
            .Returns(Array.Empty<Income>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }
}