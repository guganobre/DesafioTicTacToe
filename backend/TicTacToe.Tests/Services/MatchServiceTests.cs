namespace TicTacToe.Tests.Services;

using TicTacToe.Application.Services;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Interfaces.Services;

public class MatchServiceTests
{
    private readonly IMatchService _sut = new MatchService();

    [Fact]
    public void Create_WithValidNames_ReturnsMatchInProgress()
    {
        var match = _sut.Create("Alice", "Bob");

        Assert.NotEqual(Guid.Empty, match.Id);
        Assert.Equal("Alice", match.Player1Name);
        Assert.Equal("Bob", match.Player2Name);
        Assert.Equal(GameResult.InProgress, match.Result);
        Assert.Null(match.Winner);
    }

    [Theory]
    [InlineData("", "Bob")]
    [InlineData("Alice", "")]
    [InlineData("   ", "Bob")]
    [InlineData("Alice", "   ")]
    public void Create_WithBlankNames_ThrowsArgumentException(string player1, string player2)
    {
        var act = () => _sut.Create(player1, player2);

        Assert.Throws<ArgumentException>(act);
    }

    [Theory]
    [InlineData(null, "Bob")]
    [InlineData("Alice", null)]
    public void Create_WithNullNames_ThrowsArgumentNullException(string? player1, string? player2)
    {
        var act = () => _sut.Create(player1!, player2!);

        Assert.Throws<ArgumentNullException>(act);
    }

    [Fact]
    public void Finish_WithInProgressMatch_SetsResultAndWinner()
    {
        var match = _sut.Create("Alice", "Bob");

        _sut.Finish(match, GameResult.WinnerX, "Alice");

        Assert.Equal(GameResult.WinnerX, match.Result);
        Assert.Equal("Alice", match.Winner);
    }

    [Fact]
    public void Finish_WithDraw_SetsResultAndNullWinner()
    {
        var match = _sut.Create("Alice", "Bob");

        _sut.Finish(match, GameResult.Draw);

        Assert.Equal(GameResult.Draw, match.Result);
        Assert.Null(match.Winner);
    }

    [Fact]
    public void Finish_WhenMatchAlreadyFinished_ThrowsInvalidOperationException()
    {
        var match = _sut.Create("Alice", "Bob");
        _sut.Finish(match, GameResult.Draw);

        var act = () => _sut.Finish(match, GameResult.WinnerX, "Alice");

        Assert.Throws<InvalidOperationException>(act);
    }

    [Fact]
    public void CreateMove_WithValidData_ReturnsMove()
    {
        var matchId = Guid.NewGuid();

        var move = _sut.CreateMove(matchId, PlayerSymbol.X, 4, 1);

        Assert.NotEqual(Guid.Empty, move.Id);
        Assert.Equal(matchId, move.MatchId);
        Assert.Equal(PlayerSymbol.X, move.Player);
        Assert.Equal(4, move.Position);
        Assert.Equal(1, move.MoveOrder);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(9)]
    public void CreateMove_WithInvalidPosition_ThrowsArgumentOutOfRangeException(int position)
    {
        var act = () => _sut.CreateMove(Guid.NewGuid(), PlayerSymbol.X, position, 1);

        Assert.Throws<ArgumentOutOfRangeException>(act);
    }

    [Fact]
    public void AddMove_AddsMovesToMatch()
    {
        var match = _sut.Create("Alice", "Bob");
        var move = _sut.CreateMove(match.Id, PlayerSymbol.X, 0, 1);

        _sut.AddMove(match, move);

        Assert.Single(match.Moves);
        Assert.Contains(move, match.Moves);
    }

    [Fact]
    public void AddMove_WithMultipleMoves_ContainsAllMoves()
    {
        var match = _sut.Create("Alice", "Bob");
        var move1 = _sut.CreateMove(match.Id, PlayerSymbol.X, 0, 1);
        var move2 = _sut.CreateMove(match.Id, PlayerSymbol.O, 4, 2);
        var move3 = _sut.CreateMove(match.Id, PlayerSymbol.X, 8, 3);

        _sut.AddMove(match, move1);
        _sut.AddMove(match, move2);
        _sut.AddMove(match, move3);

        Assert.Equal(3, match.Moves.Count);
        Assert.Contains(move1, match.Moves);
        Assert.Contains(move2, match.Moves);
        Assert.Contains(move3, match.Moves);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(8)]
    public void CreateMove_WithBoundaryPositions_ReturnsMove(int position)
    {
        var matchId = Guid.NewGuid();

        var move = _sut.CreateMove(matchId, PlayerSymbol.X, position, 1);

        Assert.Equal(position, move.Position);
    }
}
