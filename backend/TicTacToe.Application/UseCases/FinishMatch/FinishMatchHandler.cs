namespace TicTacToe.Application.UseCases.FinishMatch;

using MediatR;
using TicTacToe.Application.DTOs;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Exceptions;
using TicTacToe.Domain.Interfaces.Repositories;
using TicTacToe.Domain.Interfaces.Services;

public class FinishMatchHandler(
    IMatchRepository matchRepository,
    IGameService gameService,
    IMatchService matchService)
    : IRequestHandler<FinishMatchCommand, MatchDto>
{
    public async Task<MatchDto> Handle(FinishMatchCommand command, CancellationToken ct)
    {
        var match = await matchRepository.GetByIdAsync(command.MatchId, ct)
            ?? throw new DomainException($"Partida {command.MatchId} não encontrada.");

        var result = gameService.CheckWinner(command.Board);

        if (result == GameResult.InProgress)
            throw new DomainException("O jogo ainda não terminou.");

        var winner = result switch
        {
            GameResult.WinnerX => match.Player1Name,
            GameResult.WinnerO => match.Player2Name,
            _ => null
        };

        matchService.Finish(match, result, winner);
        await matchRepository.UpdateAsync(match, ct);

        return new MatchDto(match.Id, match.Player1Name, match.Player2Name,
            match.Result, match.Winner, match.CreatedAt, []);
    }
}