namespace TicTacToe.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Application.DTOs;
using TicTacToe.Application.UseCases.CreateMatch;
using TicTacToe.Application.UseCases.FinishMatch;
using TicTacToe.Application.UseCases.GetLastMatch;
using TicTacToe.Application.UseCases.GetMatchHistory;

[ApiController]
[Route("api/[controller]")]
public class MatchesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<MatchDto>> Create(
        [FromBody] CreateMatchCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}/finish")]
    public async Task<ActionResult<MatchDto>> Finish(
        Guid id,
        [FromBody] string?[] board,
        CancellationToken ct)
    {
        var result = await mediator.Send(new FinishMatchCommand(id, board), ct);
        return Ok(result);
    }

    [HttpGet("last")]
    public async Task<ActionResult<MatchDto>> GetLast(CancellationToken ct)
    {
        var result = await mediator.Send(new GetLastMatchQuery(), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetMatchHistoryQuery(), ct);
        return Ok(result);
    }
}