namespace TicTacToe.Tests.Services;

using TicTacToe.Application.Services;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Interfaces.Services;

public class GameServiceTests
{
    private readonly IGameService _sut = new GameService();

    [Theory]
    [InlineData(0)]
    [InlineData(8)]
    [InlineData(10)]
    public void CheckWinner_WhenBoardDoesNotHaveNinePositions_ThrowsArgumentException(int size)
    {
        var board = new string?[size];

        Action act = () => _sut.CheckWinner(board);

        Assert.Throws<ArgumentException>(act);
    }

    [Theory]
    [InlineData(0, 1, 2)]
    [InlineData(3, 4, 5)]
    [InlineData(6, 7, 8)]
    [InlineData(0, 3, 6)]
    [InlineData(1, 4, 7)]
    [InlineData(2, 5, 8)]
    [InlineData(0, 4, 8)]
    [InlineData(2, 4, 6)]
    public void CheckWinner_WhenXWins_ReturnsWinnerX(int a, int b, int c)
    {
        var board = new string?[9];
        board[a] = "X";
        board[b] = "X";
        board[c] = "X";

        var result = _sut.CheckWinner(board);

        Assert.Equal(GameResult.WinnerX, result);
    }

    [Theory]
    [InlineData(0, 1, 2)]
    [InlineData(3, 4, 5)]
    [InlineData(6, 7, 8)]
    [InlineData(0, 3, 6)]
    [InlineData(1, 4, 7)]
    [InlineData(2, 5, 8)]
    [InlineData(0, 4, 8)]
    [InlineData(2, 4, 6)]
    public void CheckWinner_WhenOWins_ReturnsWinnerO(int a, int b, int c)
    {
        var board = new string?[9];
        board[a] = "O";
        board[b] = "O";
        board[c] = "O";

        var result = _sut.CheckWinner(board);

        Assert.Equal(GameResult.WinnerO, result);
    }

    [Fact]
    public void CheckWinner_WhenBoardIsFullWithoutWinner_ReturnsDraw()
    {
        // X O X
        // X X O
        // O X O
        var board = new string?[] { "X", "O", "X", "X", "X", "O", "O", "X", "O" };

        var result = _sut.CheckWinner(board);

        Assert.Equal(GameResult.Draw, result);
    }

    [Fact]
    public void CheckWinner_WhenBoardHasEmptyCells_ReturnsInProgress()
    {
        var board = new string?[] { "X", null, null, null, null, null, null, null, null };

        var result = _sut.CheckWinner(board);

        Assert.Equal(GameResult.InProgress, result);
    }

    [Fact]
    public void CheckWinner_WhenBoardIsEmpty_ReturnsInProgress()
    {
        var board = new string?[9];

        var result = _sut.CheckWinner(board);

        Assert.Equal(GameResult.InProgress, result);
    }
}
