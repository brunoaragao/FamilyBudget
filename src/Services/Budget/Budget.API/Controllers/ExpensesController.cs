using Budget.Application.Dtos;
using Budget.Application.Errors;
using Budget.Application.Requests;

using FluentResults;

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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ExpenseDto>> CreateAsync([FromBody] ExpenseDto dto)
    {
        var request = new CreateExpenseRequest(dto);
        var result = await _mediator.Send(request);

        if (result.HasError<ValidationError>())
            return ValidationProblem();

        if (result.HasError<ConflictError>())
            return Conflict();

        return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Value.Id }, result.Value);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExpenseDto>> DeleteAsync(int id)
    {
        var request = new DeleteExpenseRequest(id);
        var result = await _mediator.Send(request);

        if (result.HasError<ValidationError>())
            return ValidationProblem();

        if (result.HasError<NotFoundError>())
            return NotFound();

        return Ok(result.Value);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<ExpenseDto>> GetByDescriptionOrGetAllAsync(string? description = null)
    {
        IRequest<Result<IEnumerable<ExpenseDto>>> request = (description is not null)
            ? new GetExpensesByDescriptionSnippetRequest(description)
            : new GetExpensesRequest();

        var result = await _mediator.Send(request);

        return result.Value;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var request = new GetExpenseByIdRequest(id);
        var result = await _mediator.Send(request);

        if (result.HasError<ValidationError>())
            return ValidationProblem();

        if (result.HasError<NotFoundError>())
            return NotFound();

        return Ok(result.Value);
    }

    [HttpGet("{year}/{month}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetByMonthAsync(int month, int year)
    {
        var request = new GetExpensesByMonthRequest(month, year);
        var result = await _mediator.Send(request);

        if (result.HasError<ValidationError>())
            return ValidationProblem();

        return Ok(result.Value);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAsync([FromBody] ExpenseDto dto)
    {
        var command = new UpdateExpenseRequest(dto);
        var result = await _mediator.Send(command);

        if (result.HasError<ValidationError>())
            return ValidationProblem();

        if (result.HasError<NotFoundError>())
            return NotFound();

        if (result.HasError<ConflictError>())
            return Conflict();

        return Ok(result.Value);
    }
}