using AutoMapper;

using Budget.Application.Queries.Handlers;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.IncomeAggregates;

namespace Budget.UnitTests.Application;

public class GetIncomeByIdRequestHandlerTest
{
    readonly Mock<IIncomeRepository> _repositoryMock = new();
    readonly GetIncomeByIdRequestHandler _handler;
    readonly GetIncomeByIdRequest _defaultRequest = new(Id: 1);

    public GetIncomeByIdRequestHandlerTest()
    {
        IIncomeRepository repository = _repositoryMock.Object;

        var config = new MapperConfiguration(cfg => cfg.AddMaps("Budget.Application"));
        var mapper = config.CreateMapper();

        _handler = new(repository, mapper);

        var fakeIncome = new Income(repository, 1M, "Test", default);

        _repositoryMock
            .Setup(r => r.GetIncomeById(It.IsAny<int>()))
            .Returns(fakeIncome);

        _repositoryMock
            .Setup(r => r.ExistsIncomeWithId(It.IsAny<int>()))
            .Returns(true);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccess()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnIncome()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Handle_WhenIncomeDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(r => r.ExistsIncomeWithId(request.Id))
            .Returns(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
    }
}