using AutoMapper;

using Budget.Application.Errors;
using Budget.Application.Handlers;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;

namespace Budget.UnitTests.Application;

public class CreateIncomeRequestHandlerTest
{
    readonly Mock<IIncomeRepository> _repositoryMock = new();
    readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    readonly CreateIncomeRequestHandler _handler;
    readonly CreateIncomeRequest _defaultRequest = new(Dto: new() { Id = 0, Amount = 1M, Description = "Test", Date = default });

    public CreateIncomeRequestHandlerTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps("Budget.Application"));
        var mapper = config.CreateMapper();

        _handler = new(_repositoryMock.Object, _unitOfWorkMock.Object, mapper);
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
    public async Task Handle_ByDefault_ShouldAddIncomeToRepository()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.AddIncome(It.IsAny<Income>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldCommitUnitOfWork()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenIncomeWithSameDescriptionAndMonthAlreadyExists_ShouldReturnFailedResultWithConflictError()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithDescriptionAndMonth(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.True(result.HasError<ConflictError>());
    }

    [Fact]
    public async Task Handle_WhenIncomeWithSameDescriptionAndMonthAlreadyExists_ShouldNotAddIncomeToRepository()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithDescriptionAndMonth(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.AddIncome(It.IsAny<Income>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenIncomeWithSameDescriptionAndMonthAlreadyExists_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithDescriptionAndMonth(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
            .Returns(true);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Never);
    }
}