namespace TicTacToe.Application.Services;

using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Interfaces.Services;

public class MatchService : IMatchService
{
    public Match Create(string player1Name, string player2Name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(player1Name);
        ArgumentException.ThrowIfNullOrWhiteSpace(player2Name);

        return new Match
        {
            Id = Guid.NewGuid(),
            Player1Name = player1Name,
            Player2Name = player2Name,
            Result = GameResult.InProgress,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Finish(Match match, GameResult result, string? winner = null)
    {
        if (match.Result != GameResult.InProgress)
            throw new InvalidOperationException("Partida já foi finalizada.");

        match.Result = result;
        match.Winner = winner;
    }

    public void AddMove(Match match, Move move) => match.Moves.Add(move);

    public Move CreateMove(Guid matchId, PlayerSymbol player, int position, int moveOrder)
    {
        if (position < 0 || position > 8)
            throw new ArgumentOutOfRangeException(nameof(position), "Posição deve ser entre 0 e 8.");

        return new Move
        {
            Id = Guid.NewGuid(),
            MatchId = matchId,
            Player = player,
            Position = position,
            MoveOrder = moveOrder,
            PlayedAt = DateTime.UtcNow
        };
    }
}