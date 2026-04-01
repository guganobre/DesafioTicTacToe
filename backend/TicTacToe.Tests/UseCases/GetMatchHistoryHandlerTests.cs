namespace TicTacToe.Tests.UseCases;

using Moq;
using TicTacToe.Application.UseCases.GetMatchHistory;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Interfaces.Repositories;
using Match = TicTacToe.Domain.Entities.Match;
using Move = TicTacToe.Domain.Entities.Move;

public class GetMatchHistoryHandlerTests
{
    private readonly Mock<IMatchRepository> _matchRepositoryMock = new();
    private readonly Mock<IMoveRepository> _moveRepositoryMock = new();
    private readonly GetMatchHistoryHandler _sut;

    public GetMatchHistoryHandlerTests()
    {
        _sut = new GetMatchHistoryHandler(_matchRepositoryMock.Object, _moveRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenNoMatchesExist_ReturnsEmptyList()
    {
        _matchRepositoryMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync([]);

        var result = await _sut.HandleAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task HandleAsync_WhenMatchesExist_ReturnsMappedMatchDtos()
    {
        var matchId = Guid.NewGuid();
        var match = new Match { Id = matchId, Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.Draw, CreatedAt = DateTime.UtcNow };

        _matchRepositoryMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync([match]);
        _moveRepositoryMock.Setup(r => r.GetByMatchIdAsync(matchId, default)).ReturnsAsync([]);

        var result = (await _sut.HandleAsync()).ToList();

        Assert.Single(result);
        Assert.Equal(matchId, result[0].Id);
        Assert.Equal("Alice", result[0].Player1Name);
        Assert.Equal("Bob", result[0].Player2Name);
        Assert.Equal(GameResult.Draw, result[0].Result);
    }

    [Fact]
    public async Task HandleAsync_ReturnsMatchesOrderedByCreatedAtDescending()
    {
        var older = new Match { Id = Guid.NewGuid(), Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.Draw, CreatedAt = DateTime.UtcNow.AddHours(-1) };
        var newer = new Match { Id = Guid.NewGuid(), Player1Name = "Charlie", Player2Name = "Dave", Result = GameResult.WinnerX, CreatedAt = DateTime.UtcNow };

        _matchRepositoryMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync([older, newer]);
        _moveRepositoryMock.Setup(r => r.GetByMatchIdAsync(It.IsAny<Guid>(), default)).ReturnsAsync([]);

        var result = (await _sut.HandleAsync()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal(newer.Id, result[0].Id);
        Assert.Equal(older.Id, result[1].Id);
    }

    [Fact]
    public async Task HandleAsync_MapsMovesForEachMatch()
    {
        var matchId = Guid.NewGuid();
        var match = new Match { Id = matchId, Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.InProgress, CreatedAt = DateTime.UtcNow };
        Move[] moves =
        [
            new() { Id = Guid.NewGuid(), MatchId = matchId, Player = PlayerSymbol.X, Position = 0, MoveOrder = 1, PlayedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), MatchId = matchId, Player = PlayerSymbol.O, Position = 4, MoveOrder = 2, PlayedAt = DateTime.UtcNow }
        ];

        _matchRepositoryMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync([match]);
        _moveRepositoryMock.Setup(r => r.GetByMatchIdAsync(matchId, default)).ReturnsAsync(moves);

        var result = (await _sut.HandleAsync()).ToList();

        Assert.Single(result);
        Assert.Equal(2, result[0].Moves.Count());
    }

    [Fact]
    public async Task HandleAsync_CallsMoveRepositoryForEachMatch()
    {
        var matchId1 = Guid.NewGuid();
        var matchId2 = Guid.NewGuid();
        var match1 = new Match { Id = matchId1, Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.Draw, CreatedAt = DateTime.UtcNow.AddHours(-1) };
        var match2 = new Match { Id = matchId2, Player1Name = "Charlie", Player2Name = "Dave", Result = GameResult.WinnerX, CreatedAt = DateTime.UtcNow };

        _matchRepositoryMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync([match1, match2]);
        _moveRepositoryMock.Setup(r => r.GetByMatchIdAsync(It.IsAny<Guid>(), default)).ReturnsAsync([]);

        await _sut.HandleAsync();

        _moveRepositoryMock.Verify(r => r.GetByMatchIdAsync(matchId1, default), Times.Once);
        _moveRepositoryMock.Verify(r => r.GetByMatchIdAsync(matchId2, default), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithCancellationToken_PropagatesTokenToRepositories()
    {
        var matchId = Guid.NewGuid();
        var match = new Match { Id = matchId, Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.Draw, CreatedAt = DateTime.UtcNow };
        using var cts = new CancellationTokenSource();

        _matchRepositoryMock.Setup(r => r.GetAllAsync(cts.Token)).ReturnsAsync([match]);
        _moveRepositoryMock.Setup(r => r.GetByMatchIdAsync(matchId, cts.Token)).ReturnsAsync([]);

        await _sut.HandleAsync(cts.Token);

        _matchRepositoryMock.Verify(r => r.GetAllAsync(cts.Token), Times.Once);
        _moveRepositoryMock.Verify(r => r.GetByMatchIdAsync(matchId, cts.Token), Times.Once);
    }
}
