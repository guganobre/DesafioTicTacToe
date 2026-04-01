namespace TicTacToe.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Application.DTOs;
using TicTacToe.Application.UseCases.RegisterMove;

[ApiController]
[Route("api/matches/{matchId:guid}/[controller]")]
public class MovesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<MoveDto>> Register(
        Guid matchId,
        [FromBody] RegisterMoveCommand command,
        CancellationToken ct)
    {
        var result = await mediator.Send(command with { MatchId = matchId }, ct);
        return CreatedAtAction(nameof(Register), result);
    }
}
