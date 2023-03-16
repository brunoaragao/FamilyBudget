using Budget.Application.Commands;
using Budget.Application.Dtos;
using Budget.Application.Queries;

using FluentResults;
using FluentResults.Extensions.AspNetCore;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class IncomesController : ControllerBase
{
    private readonly IMediator _mediator;

    public IncomesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] IncomeDto dto)
    {
        var command = new CreateIncomeCommand(dto);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteIncomeCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetByDescriptionOrGetAll(string? description = null)
    {
        IRequest<Result<IEnumerable<IncomeDto>>> query = (description is not null)
            ? new GetIncomesByDescriptionSnippetQuery(description)
            : new GetIncomesQuery();

        var result = await _mediator.Send(query);
        return result.ToActionResult();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetIncomeByIdQuery(id);
        var result = await _mediator.Send(query);
        return result.ToActionResult();
    }

    [HttpGet("{year}/{month}")]
    public async Task<IActionResult> GetByMonth(int month, int year)
    {
        var query = new GetIncomesByMonthQuery(year, month);
        var result = await _mediator.Send(query);
        return result.ToActionResult();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] IncomeDto dto)
    {
        var command = new UpdateIncomeCommand(dto);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
}