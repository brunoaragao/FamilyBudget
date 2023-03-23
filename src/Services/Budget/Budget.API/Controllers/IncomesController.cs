using Budget.Application.Dtos;
using Budget.Application.Errors;
using Budget.Application.Requests;

using FluentResults;

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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<IncomeDto>> CreateAsync([FromBody] IncomeDto dto)
    {
        var request = new CreateIncomeRequest(dto);
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
    public async Task<ActionResult<IncomeDto>> DeleteAsync(int id)
    {
        var request = new DeleteIncomeRequest(id);
        var result = await _mediator.Send(request);

        if (result.HasError<ValidationError>())
            return ValidationProblem();

        if (result.HasError<NotFoundError>())
            return NotFound();

        return Ok(result.Value);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<IncomeDto>> GetByDescriptionOrGetAllAsync(string? description = null)
    {
        IRequest<Result<IEnumerable<IncomeDto>>> request = (description is not null)
            ? new GetIncomesByDescriptionSnippetRequest(description)
            : new GetIncomesRequest();

        var result = await _mediator.Send(request);

        return result.Value;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var request = new GetIncomeByIdRequest(id);
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
    public async Task<ActionResult<IEnumerable<IncomeDto>>> GetByMonthAsync(int month, int year)
    {
        var request = new GetIncomesByMonthRequest(month, year);
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
    public async Task<IActionResult> UpdateAsync([FromBody] IncomeDto dto)
    {
        var command = new UpdateIncomeRequest(dto);
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