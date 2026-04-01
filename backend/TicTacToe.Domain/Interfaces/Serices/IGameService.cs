namespace TicTacToe.Domain.Interfaces.Services;

using TicTacToe.Domain.Enums;

public interface IGameService
{
    GameResult CheckWinner(string?[] board);
}