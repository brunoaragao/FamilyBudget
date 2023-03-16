using Budget.Application.Dtos;
using Budget.Application.Queries;
using Budget.Application.Queries.Handlers;
using Budget.Domain.Services;

using FluentResults;

using FluentValidation;
using FluentValidation.Results;

using MediatR;

namespace Budget.Application.Commands.Handlers;

public class GetMonthlySummaryQueryHandlerTest
{
    private readonly Mock<IBudgetSummaryService> _budgetSummaryServiceMock;
    private readonly Mock<IValidator<GetMonthlySummaryQuery>> _validatorMock;
    private readonly IRequestHandler<GetMonthlySummaryQuery, Result<MonthlyBudgetSummaryDto>> _handler;

    public GetMonthlySummaryQueryHandlerTest()
    {
        _budgetSummaryServiceMock = new Mock<IBudgetSummaryService>();
        _validatorMock = new Mock<IValidator<GetMonthlySummaryQuery>>();

        _validatorMock
            .Setup(v => v.Validate(It.IsAny<GetMonthlySummaryQuery>()))
            .Returns(new ValidationResult());

        _budgetSummaryServiceMock
            .Setup(s => s.GetMonthlySummary(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(GetDefaultMonthlySummary());

        _handler = new GetMonthlySummaryQueryHandler(_budgetSummaryServiceMock.Object, _validatorMock.Object);
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
    public async Task Handle_WhenDoesNotExistsIncomesOrExpensesWithMonth_ShouldReturnSuccessResultWithEmptyValues()
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
    public async Task Handle_WhenQueryIsInvalid_ShouldReturnFailResult()
    {
        // Arrange
        var query = GetDefaultQuery();

        _validatorMock
            .Setup(v => v.Validate(It.IsAny<GetMonthlySummaryQuery>()))
            .Returns(GetFailureValidationResultWithError());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.NotNull(result.Errors);
    }

    private static MonthlyBudgetSummaryModel GetDefaultMonthlySummary()
    {
        return new MonthlyBudgetSummaryModel
        {
            Month = 1,
            Year = 0001,
            IncomeSum = 0M,
            ExpenseSum = 0M,
            Balance = 0M,
            CategorizedExpenses = Enumerable.Empty<CategorizedExpenseModel>()
        };
    }

    private static GetMonthlySummaryQuery GetDefaultQuery()
    {
        return new GetMonthlySummaryQuery(1, 0001);
    }

    private static ValidationResult GetFailureValidationResultWithError()
    {
        return new ValidationResult(new List<ValidationFailure> { new ValidationFailure() });
    }
}