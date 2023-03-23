using AutoMapper;

using Budget.Application.Queries.Handlers;
using Budget.Application.Requests;
using Budget.Domain.Services;

namespace Budget.Application.Handlers;

public class GetMonthlySummaryRequestHandlerTest
{
    readonly Mock<IBudgetSummaryService> _budgetSummaryServiceMock = new();
    readonly GetMonthlySummaryRequestHandler _handler;

    readonly GetMonthlySummaryRequest _defaultRequest = new(Month: 1, Year: 1);

    public GetMonthlySummaryRequestHandlerTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddMaps("Budget.Application"));
        var mapper = config.CreateMapper();

        _handler = new(_budgetSummaryServiceMock.Object, mapper);

        var fakeMonthlySummary = new MonthlyBudgetSummaryModel
        {
            Month = 1,
            Year = 1,
            IncomeSum = 1M,
            ExpenseSum = 1M,
            Balance = 0M,
            CategorizedExpenses = Enumerable.Empty<CategorizedExpenseModel>()
        };

        _budgetSummaryServiceMock
            .Setup(s => s.GetMonthlySummary(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(fakeMonthlySummary);
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
    public async Task Handle_WhenDoesNotExistsIncomesOrExpensesWithMonth_ShouldReturnSuccessResultWithEmptyValues()
    {
        // Arrange
        var request = _defaultRequest;

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}