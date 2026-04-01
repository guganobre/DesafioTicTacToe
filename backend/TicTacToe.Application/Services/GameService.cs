namespace TicTacToe.Application.Services;

using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Interfaces.Services;

public class GameService : IGameService
{
    private static readonly int[][] WinningCombinations =
    [
        [0, 1, 2], [3, 4, 5], [6, 7, 8],
        [0, 3, 6], [1, 4, 7], [2, 5, 8],
        [0, 4, 8], [2, 4, 6]
    ];

    public GameResult CheckWinner(string?[] board)
    {
        if (board.Length != 9)
            throw new ArgumentException("O tabuleiro deve ter 9 posições.");

        foreach (var combo in WinningCombinations)
        {
            var a = board[combo[0]];
            var b = board[combo[1]];
            var c = board[combo[2]];

            if (a != null && a == b && b == c)
                return a == "X" ? GameResult.WinnerX : GameResult.WinnerO;
        }

        return board.Any(cell => cell is null)
            ? GameResult.InProgress
            : GameResult.Draw;
    }
}