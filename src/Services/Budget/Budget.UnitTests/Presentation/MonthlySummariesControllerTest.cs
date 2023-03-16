using Budget.API.Controllers;
using Budget.Application.Dtos;
using Budget.Application.Queries;

using FluentResults;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Budget.UnitTests.Presentation;

public class MonthlySummariesControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly MonthlySummariesController _controller;

    public MonthlySummariesControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new MonthlySummariesController(_mediatorMock.Object);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetMonthlySummaryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(GetDefaultDto()));
    }

    [Fact]
    public async Task GetByYearAndMonth_ByDefault_ShouldReturnSuccessResultWithDto()
    {
        // Arrange
        var month = default(int);
        var year = default(int);

        // Act
        var result = await _controller.GetByYearAndMonth(month, year);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetByYearAndMonth_WhenQueryFails_ShouldReturnBadRequestResultWithError()
    {
        // Arrange
        var month = default(int);
        var year = default(int);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetMonthlySummaryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Error"));

        // Act
        var result = await _controller.GetByYearAndMonth(month, year);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Error", badRequestResult.Value?.ToString());
    }

    private static MonthlyBudgetSummaryDto GetDefaultDto()
    {
        return new MonthlyBudgetSummaryDto
        {
            Month = default,
            Year = default,
            IncomeSum = default,
            ExpenseSum = default,
            Balance = default,
            CategorizedExpenses = Enumerable.Empty<CategorizedExpenseDto>()
        };
    }
}