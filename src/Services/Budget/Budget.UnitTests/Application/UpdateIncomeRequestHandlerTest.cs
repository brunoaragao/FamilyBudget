using AutoMapper;

using Budget.Application.Errors;
using Budget.Application.Handlers;
using Budget.Application.Requests;
using Budget.Domain.AggregateModels.IncomeAggregates;
using Budget.Domain.SeedWork;

namespace Budget.UnitTests.Application;

public class UpdateIncomeRequestHandlerTest
{
    readonly Mock<IIncomeRepository> _repositoryMock = new();
    readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    readonly UpdateIncomeRequestHandler _handler;
    readonly UpdateIncomeRequest _defaultRequest = new(Dto: new() { Id = 1, Amount = 1M, Description = "Test", Date = default });

    public UpdateIncomeRequestHandlerTest()
    {
        var repository = _repositoryMock.Object;

        var fakeIncome = new Income(repository, 1M, "Test", default);

        var config = new MapperConfiguration(cfg => cfg.AddMaps("Budget.Application"));
        var mapper = config.CreateMapper();

        _handler = new UpdateIncomeRequestHandler(repository, _unitOfWorkMock.Object, mapper);

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(It.IsAny<int>()))
            .Returns(true);

        _repositoryMock
            .Setup(x => x.GetIncomeById(It.IsAny<int>()))
            .Returns(fakeIncome);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ShouldCommitUnitOfWork()
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
            .Setup(x => x.ExistsIncomeWithId(request.Dto.Id))
            .Returns(false);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.True(result.HasError<NotFoundError>());
    }

    [Fact]
    public async Task Handle_WhenIncomeDoesNotExist_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var request = _defaultRequest;

        _repositoryMock
            .Setup(x => x.ExistsIncomeWithId(request.Dto.Id))
            .Returns(false);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenExistsAnotherIncomeWithDescriptionAndMonth_ShouldReturnFailedResultWithConflictError()
    {
        // Arrange
        var request = _defaultRequest;
        var (id, amount, description, date) = request.Dto;

        _repositoryMock
            .Setup(x => x.ExistsAnotherIncomeWithDescriptionAndMonth(id, description, date.Month, date.Year))
            .Returns(true);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.True(result.HasError<ConflictError>());
    }

    [Fact]
    public async Task Handle_WhenExistsAnotherIncomeWithDescriptionAndMonth_ShouldNotCommitUnitOfWork()
    {
        // Arrange
        var request = _defaultRequest;
        var (id, amount, description, date) = request.Dto;

        _repositoryMock
            .Setup(x => x.ExistsAnotherIncomeWithDescriptionAndMonth(id, description, date.Month, date.Year))
            .Returns(true);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _unitOfWorkMock.Verify(x => x.Commit(), Times.Never);
    }
}