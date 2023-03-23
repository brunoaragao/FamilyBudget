using AutoMapper;

using Budget.Application.Errors;
using Budget.Application.Handlers;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;

namespace Budget.UnitTests.Application;

public class DeleteIncomeRequestHandlerTest
{
    readonly Mock<IIncomeRepository> _repositoryMock = new();
    readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    readonly DeleteIncomeRequestHandler _handler;
    readonly DeleteIncomeRequest _defaultRequest = new(Id: 1);

    public DeleteIncomeRequestHandlerTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps("Budget.Application"));
        var mapper = config.CreateMapper();

        _handler = new(_repositoryMock.Object, _unitOfWorkMock.Object, mapper);

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(It.IsAny<int>()))
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
    public async Task Handle_ByDefault_ShouldDeleteIncome()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.DeleteIncome(It.IsAny<Income>()), Times.Once);
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
    public async Task Handle_WhenIncomeDoesNotExist_ShouldReturnFailedResultWithNotFoundError()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(request.Id))
            .Returns(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.True(result.HasError<NotFoundError>());
    }

    [Fact]
    public async Task Handle_WhenIncomeDoesNotExist_ShouldNotDeleteIncome()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(request.Id))
            .Returns(false);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.DeleteIncome(It.IsAny<Income>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenIncomeDoesNotExist_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(request.Id))
            .Returns(false);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Never);
    }
}