namespace TicTacToe.Tests.UseCases;

using Moq;
using TicTacToe.Application.UseCases.GetLastMatch;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Interfaces.Repositories;
using Match = TicTacToe.Domain.Entities.Match;
using Move = TicTacToe.Domain.Entities.Move;

public class GetLastMatchHandlerTests
{
    private readonly Mock<IMatchRepository> _matchRepositoryMock = new();
    private readonly GetLastMatchHandler _sut;

    public GetLastMatchHandlerTests()
    {
        _sut = new GetLastMatchHandler(_matchRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenNoMatchExists_ReturnsNull()
    {
        _matchRepositoryMock.Setup(r => r.GetLastAsync(default)).ReturnsAsync((Match?)null);

        var result = await _sut.Handle(new GetLastMatchQuery(), default);

        Assert.Null(result);
    }

    [Fact]
    public async Task HandleAsync_WhenMatchExists_ReturnsMappedMatchDto()
    {
        var match = new Match
        {
            Id = Guid.NewGuid(),
            Player1Name = "Alice",
            Player2Name = "Bob",
            Result = GameResult.WinnerX,
            Winner = "Alice",
            CreatedAt = DateTime.UtcNow
        };

        _matchRepositoryMock.Setup(r => r.GetLastAsync(default)).ReturnsAsync(match);

        var result = await _sut.Handle(new GetLastMatchQuery(), default);

        Assert.NotNull(result);
        Assert.Equal(match.Id, result.Id);
        Assert.Equal("Alice", result.Player1Name);
        Assert.Equal("Bob", result.Player2Name);
        Assert.Equal(GameResult.WinnerX, result.Result);
        Assert.Equal("Alice", result.Winner);
    }

    [Fact]
    public async Task HandleAsync_WhenMatchHasMoves_ReturnsMovesSortedByMoveOrder()
    {
        var matchId = Guid.NewGuid();
        var match = new Match
        {
            Id = matchId,
            Player1Name = "Alice",
            Player2Name = "Bob",
            Result = GameResult.InProgress,
            CreatedAt = DateTime.UtcNow,
            Moves =
            [
                new Move { Id = Guid.NewGuid(), MatchId = matchId, Player = PlayerSymbol.O, Position = 4, MoveOrder = 2, PlayedAt = DateTime.UtcNow },
                new Move { Id = Guid.NewGuid(), MatchId = matchId, Player = PlayerSymbol.X, Position = 0, MoveOrder = 1, PlayedAt = DateTime.UtcNow }
            ]
        };

        _matchRepositoryMock.Setup(r => r.GetLastAsync(default)).ReturnsAsync(match);

        var result = await _sut.Handle(new GetLastMatchQuery(), default);

        Assert.NotNull(result);
        var moves = result.Moves.ToList();
        Assert.Equal(2, moves.Count);
        Assert.Equal(1, moves[0].MoveOrder);
        Assert.Equal(2, moves[1].MoveOrder);
    }

    [Fact]
    public async Task HandleAsync_WhenMatchHasNoMoves_ReturnsEmptyMovesList()
    {
        var match = new Match
        {
            Id = Guid.NewGuid(),
            Player1Name = "Alice",
            Player2Name = "Bob",
            Result = GameResult.InProgress,
            CreatedAt = DateTime.UtcNow
        };

        _matchRepositoryMock.Setup(r => r.GetLastAsync(default)).ReturnsAsync(match);

        var result = await _sut.Handle(new GetLastMatchQuery(), default);

        Assert.NotNull(result);
        Assert.Empty(result.Moves);
    }

    [Fact]
    public async Task HandleAsync_WithCancellationToken_PropagatesToken()
    {
        using var cts = new CancellationTokenSource();

        _matchRepositoryMock.Setup(r => r.GetLastAsync(cts.Token)).ReturnsAsync((Match?)null);

        await _sut.Handle(new GetLastMatchQuery(), cts.Token);

        _matchRepositoryMock.Verify(r => r.GetLastAsync(cts.Token), Times.Once);
    }
}
