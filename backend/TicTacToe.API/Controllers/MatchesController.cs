namespace TicTacToe.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using TicTacToe.Application.DTOs;
using TicTacToe.Application.UseCases.CreateMatch;
using TicTacToe.Application.UseCases.FinishMatch;
using TicTacToe.Application.UseCases.GetLastMatch;
using TicTacToe.Application.UseCases.GetMatchHistory;

[ApiController]
[Route("api/[controller]")]
public class MatchesController(
    CreateMatchHandler createMatchHandler,
    FinishMatchHandler finishMatchHandler,
    GetMatchHistoryHandler getMatchHistoryHandler,
    GetLastMatchHandler getLastMatchHandler) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<MatchDto>> Create(
        [FromBody] CreateMatchCommand command,
        CancellationToken ct)
    {
        var result = await createMatchHandler.HandleAsync(command, ct);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}/finish")]
    public async Task<ActionResult<MatchDto>> Finish(
        Guid id,
        [FromBody] string?[] board,
        CancellationToken ct)
    {
        var result = await finishMatchHandler.HandleAsync(new FinishMatchCommand(id, board), ct);
        return Ok(result);
    }

    [HttpGet("last")]
    public async Task<ActionResult<MatchDto>> GetLast(CancellationToken ct)
    {
        var result = await getLastMatchHandler.HandleAsync(ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetAll(CancellationToken ct)
    {
        var result = await getMatchHistoryHandler.HandleAsync(ct);
        return Ok(result);
    }
}