using Budget.Application.Dtos;
using Budget.Application.Queries;
using Budget.Application.Queries.Handlers;
using Budget.Application.Validators;
using Budget.Domain.AggregateModels.IncomeAggregates;

using FluentResults;

using FluentValidation;

using MediatR;

namespace Budget.UnitTests.Application;

public class GetIncomeByIdQueryHandlerTest
{
    private readonly Mock<IIncomeRepository> _repositoryMock;
    private readonly IIncomeRepository _repository;
    private readonly IValidator<GetIncomeByIdQuery> _validator;
    private readonly IRequestHandler<GetIncomeByIdQuery, Result<IncomeDto>> _handler;

    public GetIncomeByIdQueryHandlerTest()
    {
        _repositoryMock = new Mock<IIncomeRepository>();
        _repository = _repositoryMock.Object;

        _validator = new GetIncomeByIdQueryValidator(_repository);

        _handler = new GetIncomeByIdQueryHandler(_repository, _validator);

        _repositoryMock
            .Setup(r => r.GetIncomeById(It.IsAny<int>()))
            .Returns(GetDefaultIncome());

        _repositoryMock
            .Setup(r => r.ExistsIncomeWithId(It.IsAny<int>()))
            .Returns(true);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnSuccess()
    {
        // Arrange
        var query = GetDefaultQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ByDefault_ShouldReturnIncome()
    {
        // Arrange
        var query = GetDefaultQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Handle_WhenIncomeDoesNotExist_ShouldReturnFailure()
    {
        // Arrange
        var query = GetDefaultQuery();

        _repositoryMock
            .Setup(r => r.ExistsIncomeWithId(query.Id))
            .Returns(false);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
    }

    private static GetIncomeByIdQuery GetDefaultQuery()
    {
        return new GetIncomeByIdQuery(1);
    }

    private Income GetDefaultIncome()
    {
        return new Income(_repository, 100, "Test", DateTime.UtcNow);
    }
}