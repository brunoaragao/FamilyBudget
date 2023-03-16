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
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpensesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExpenseDto dto)
    {
        var command = new CreateExpenseCommand(dto);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteExpenseCommand(id);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetByDescriptionOrGetAll(string? description = null)
    {
        IRequest<Result<IEnumerable<ExpenseDto>>> query = (description is not null)
            ? new GetExpensesByDescriptionSnippetQuery(description)
            : new GetExpensesQuery();

        var result = await _mediator.Send(query);
        return result.ToActionResult();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetExpenseByIdQuery(id);
        var result = await _mediator.Send(query);
        return result.ToActionResult();
    }

    [HttpGet("{year}/{month}")]
    public async Task<IActionResult> GetByMonth(int month, int year)
    {
        var query = new GetExpensesByMonthQuery(month, year);
        var result = await _mediator.Send(query);
        return result.ToActionResult();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ExpenseDto dto)
    {
        var command = new UpdateExpenseCommand(dto);
        var result = await _mediator.Send(command);
        return result.ToActionResult();
    }
}