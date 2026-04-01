namespace TicTacToe.Application.UseCases.RegisterMove;

using TicTacToe.Application.DTOs;
using TicTacToe.Domain.Exceptions;
using TicTacToe.Domain.Interfaces.Repositories;
using TicTacToe.Domain.Interfaces.Services;

public class RegisterMoveHandler(
    IMatchRepository matchRepository,
    IMoveRepository moveRepository,
    IMatchService matchService)
{
    public async Task<MoveDto> HandleAsync(RegisterMoveCommand command, CancellationToken ct = default)
    {
        var match = await matchRepository.GetByIdAsync(command.MatchId, ct)
            ?? throw new DomainException($"Partida {command.MatchId} não encontrada.");

        var move = matchService.CreateMove(match.Id, command.Player, command.Position, command.MoveOrder);

        await moveRepository.AddAsync(move, ct);

        return new MoveDto(move.Id, move.Player, move.Position, move.MoveOrder, move.PlayedAt);
    }
}