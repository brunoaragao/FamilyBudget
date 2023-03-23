using AutoMapper;

using Budget.Application.Queries.Handlers;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.IncomeAggregates;

namespace Budget.Application.Handlers;

public class GetIncomesByDescriptionSnippetRequestHandlerTest
{
    readonly Mock<IIncomeRepository> _repositoryMock = new();
    readonly GetIncomesByDescriptionSnippetRequestHandler _handler;
    readonly GetIncomesByDescriptionSnippetRequest _defaultRequest = new(DescriptionSnippet: "Test");

    public GetIncomesByDescriptionSnippetRequestHandlerTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps("Budget.Application"));
        var mapper = config.CreateMapper();

        IIncomeRepository repository = _repositoryMock.Object;

        Income[] fakeIncomes =
        {
            new(repository, 1M, "Test", default),
            new(repository, 1M, "Test", default)
        };

        _handler = new(repository, mapper);

        _repositoryMock
            .Setup(r => r.GetIncomesByDescriptionSnippet(It.IsAny<string>()))
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
    public async Task Handle_WhenDoesNotExistsIncomesWithDescriptionSnippet_ShouldReturnSuccessResultWithEmptyValues()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(r => r.GetIncomesByDescriptionSnippet(It.IsAny<string>()))
            .Returns(Enumerable.Empty<Income>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }
}