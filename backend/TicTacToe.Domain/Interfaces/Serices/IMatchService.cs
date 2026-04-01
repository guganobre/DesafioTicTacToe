namespace TicTacToe.Domain.Interfaces.Services;

using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Enums;

public interface IMatchService
{
    Match Create(string player1Name, string player2Name);

    void Finish(Match match, GameResult result, string? winner = null);

    void AddMove(Match match, Move move);

    Move CreateMove(Guid matchId, PlayerSymbol player, int position, int moveOrder);
}