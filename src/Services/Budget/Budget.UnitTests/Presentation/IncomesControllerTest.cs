using Budget.API.Controllers;
using Budget.Application.Commands;
using Budget.Application.Dtos;
using Budget.Application.Queries;

using FluentResults;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Budget.UnitTests.Presentation;

public class IncomesControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly IncomesController _controller;

    public IncomesControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new IncomesController(_mediatorMock.Object);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateIncomeCommand>(), CancellationToken.None))
            .ReturnsAsync(Result.Ok(default(int)));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteIncomeCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok());

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetIncomesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(GetDefaultDtos()));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetIncomesByDescriptionSnippetQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(GetDefaultDtos()));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetIncomeByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(GetDefaultDto()));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetIncomesByMonthQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Ok(GetDefaultDtos()));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateIncomeCommand>(), It.IsAny<CancellationToken>()))
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
            .Setup(m => m.Send(It.IsAny<CreateIncomeCommand>(), It.IsAny<CancellationToken>()))
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
            .Setup(m => m.Send(It.IsAny<DeleteIncomeCommand>(), It.IsAny<CancellationToken>()))
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
            .Setup(m => m.Send(It.IsAny<GetIncomesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<IEnumerable<IncomeDto>>("Error"));

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
            .Setup(m => m.Send(It.IsAny<GetIncomeByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<IncomeDto>("Error"));

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
            .Setup(m => m.Send(It.IsAny<GetIncomesByMonthQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail<IEnumerable<IncomeDto>>("Error"));

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
            .Setup(m => m.Send(It.IsAny<UpdateIncomeCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail("Error"));

        // Act
        var result = await _controller.Update(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Contains("Error", badRequestResult.Value?.ToString());
    }

    private static IncomeDto GetDefaultDto()
    {
        return new IncomeDto
        {
            Id = default,
            Amount = default,
            Description = string.Empty,
            Date = default
        };
    }

    private static IEnumerable<IncomeDto> GetDefaultDtos()
    {
        yield return GetDefaultDto();
    }
}