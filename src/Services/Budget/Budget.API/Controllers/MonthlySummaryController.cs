using Budget.Application.Dtos;
using Budget.Application.Errors;
using Budget.Application.Requests;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MonthlySummaryController : ControllerBase
{
    private readonly IMediator _mediator;

    public MonthlySummaryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{year}/{month}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MonthlyBudgetSummaryDto>> GetByMonth(int month, int year)
    {
        var query = new GetMonthlySummaryRequest(month, year);
        var result = await _mediator.Send(query);

        if (result.HasError<ValidationError>())
            return ValidationProblem();

        return Ok(result.Value);
    }
}