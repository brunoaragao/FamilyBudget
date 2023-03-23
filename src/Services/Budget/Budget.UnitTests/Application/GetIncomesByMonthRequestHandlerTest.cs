using AutoMapper;

using Budget.Application.Queries.Handlers;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.IncomeAggregates;

namespace Budget.Application.Handlers;

public class GetIncomesByMonthRequestHandlerTest
{
    readonly Mock<IIncomeRepository> _repositoryMock = new();
    readonly GetIncomesByMonthRequestHandler _handler;

    readonly GetIncomesByMonthRequest _defaultRequest = new(Month: 1, Year: 1);

    public GetIncomesByMonthRequestHandlerTest()
    {
        IIncomeRepository repository = _repositoryMock.Object;

        var config = new MapperConfiguration(cfg => cfg.AddMaps("Budget.Application"));
        var mapper = config.CreateMapper();

        _handler = new(repository, mapper);

        Income[] fakeIncomes =
        {
            new(repository, 1M, "Test", default),
            new(repository, 1M, "Test", default)
        };

        _repositoryMock
            .Setup(r => r.GetIncomesByMonth(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(fakeIncomes);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccessResultWithValues()
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
    public async Task Handle_WhenDoesNotExistsIncomesWithMonth_ShouldReturnSuccessResultWithEmptyValues()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(r => r.GetIncomesByMonth(request.Month, request.Year))
            .Returns(Enumerable.Empty<Income>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }
}