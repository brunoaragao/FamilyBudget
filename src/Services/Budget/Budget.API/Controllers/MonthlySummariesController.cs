using Budget.Application.Queries;

using FluentResults.Extensions.AspNetCore;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class MonthlySummariesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MonthlySummariesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{year}/{month}")]
    public async Task<IActionResult> GetByYearAndMonth(int month, int year)
    {
        var query = new GetMonthlySummaryQuery(month, year);
        var result = await _mediator.Send(query);
        return result.ToActionResult();
    }
}