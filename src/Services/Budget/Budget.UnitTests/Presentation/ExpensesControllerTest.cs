using Budget.API.Controllers;
using Budget.Application.Commands;
using Budget.Application.Dtos;
using Budget.Application.Queries;

using FluentResults;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Budget.UnitTests.Presentation;

public class ExpensesControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ExpensesController _controller;

    public ExpensesControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ExpensesController(_mediatorMock.Object);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateExpenseCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(default(int)));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteExpenseCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetExpensesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(GetDefaultDtos()));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetExpensesByDescriptionSnippetQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(GetDefaultDtos()));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetExpenseByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(GetDefaultDto()));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetExpensesByMonthQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(GetDefaultDtos()));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateExpenseCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());
    }

    [Fact]
    public async Task Create_ByDefault_ShouldReturnSuccess()
    {
        // Arrange
        var dto = GetDefaultDto();

        // Act
        var result = await _controller.Create(dto);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Create_WhenCommandFails_ShouldReturnBadRequestWithErrorMessage()
    {
        // Arrange
        var dto = GetDefaultDto();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateExpenseCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<int>("Error"));

        // Act
        var result = await _controller.Create(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Error", badRequestResult.Value?.ToString());
    }

    [Fact]
    public async Task Delete_ByDefault_ShouldReturnSuccess()
    {
        // Arrange
        var id = default(int);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Delete_WhenCommandFails_ShouldReturnBadRequestWithErrorMessage()
    {
        // Arrange
        var id = default(int);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteExpenseCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Error"));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Error", badRequestResult.Value?.ToString());
    }

    [Fact]
    public async Task GetByDescriptionOrGetAll_WhenDescriptionSnippedIsProvided_ShouldReturnSuccess()
    {
        // Arrange
        var description = "Test";

        // Act
        var result = await _controller.GetByDescriptionOrGetAll(description);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetByDescriptionOrGetAll_WhenDescriptionSnippedIsNotProvided_ShouldReturnSuccess()
    {
        // Act
        var result = await _controller.GetByDescriptionOrGetAll();

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetByDescription_WhenQueryFails_ShouldReturnBadRequestWithErrorMessage()
    {
        // Arrange
        var description = default(string);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetExpensesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<IEnumerable<ExpenseDto>>("Error"));

        // Act
        var result = await _controller.GetByDescriptionOrGetAll(description);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Error", badRequestResult.Value?.ToString());
    }

    [Fact]
    public async Task GetById_ByDefault_ShouldReturnSuccess()
    {
        // Arrange
        var id = default(int);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetById_WhenQueryFails_ShouldReturnBadRequestWithErrorMessage()
    {
        // Arrange
        var id = default(int);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetExpenseByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<ExpenseDto>("Error"));

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Error", badRequestResult.Value?.ToString());
    }

    [Fact]
    public async Task GetByMonth_ByDefault_ShouldReturnSuccess()
    {
        // Arrange
        var month = default(int);
        var year = default(int);

        // Act
        var result = await _controller.GetByMonth(month, year);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetByMonth_WhenQueryFails_ShouldReturnBadRequestWithErrorMessage()
    {
        // Arrange
        var month = default(int);
        var year = default(int);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetExpensesByMonthQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<IEnumerable<ExpenseDto>>("Error"));

        // Act
        var result = await _controller.GetByMonth(month, year);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Error", badRequestResult.Value?.ToString());
    }

    [Fact]
    public async Task Update_ByDefault_ShouldReturnSuccess()
    {
        // Arrange
        var dto = GetDefaultDto();

        // Act
        var result = await _controller.Update(dto);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Update_WhenCommandFails_ShouldReturnBadRequestWithErrorMessage()
    {
        // Arrange
        var dto = GetDefaultDto();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateExpenseCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Error"));

        // Act
        var result = await _controller.Update(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Error", badRequestResult.Value?.ToString());
    }

    private static ExpenseDto GetDefaultDto()
    {
        return new ExpenseDto
        {
            Id = default,
            Amount = default,
            Description = string.Empty,
            Date = default,
            ExpenseCategory = ExpenseCategoryDto.Others
        };
    }

    private static IEnumerable<ExpenseDto> GetDefaultDtos()
    {
        yield return GetDefaultDto();
    }
}