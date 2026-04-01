namespace TicTacToe.Tests.UseCases;

using Moq;
using TicTacToe.Application.UseCases.FinishMatch;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Exceptions;
using TicTacToe.Domain.Interfaces.Repositories;
using TicTacToe.Domain.Interfaces.Services;
using Match = TicTacToe.Domain.Entities.Match;

public class FinishMatchHandlerTests
{
    private readonly Mock<IMatchRepository> _matchRepositoryMock = new();
    private readonly Mock<IGameService> _gameServiceMock = new();
    private readonly Mock<IMatchService> _matchServiceMock = new();
    private readonly FinishMatchHandler _sut;

    public FinishMatchHandlerTests()
    {
        _sut = new FinishMatchHandler(
            _matchRepositoryMock.Object,
            _gameServiceMock.Object,
            _matchServiceMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WhenMatchNotFound_ThrowsDomainException()
    {
        var matchId = Guid.NewGuid();
        var command = new FinishMatchCommand(matchId, new string?[9]);

        _matchRepositoryMock.Setup(r => r.GetByIdAsync(matchId, default)).ReturnsAsync((Match?)null);

        await Assert.ThrowsAsync<DomainException>(() => _sut.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_WhenGameStillInProgress_ThrowsDomainException()
    {
        var matchId = Guid.NewGuid();
        var board = new string?[9];
        var command = new FinishMatchCommand(matchId, board);
        var match = new Match { Id = matchId, Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.InProgress, CreatedAt = DateTime.UtcNow };

        _matchRepositoryMock.Setup(r => r.GetByIdAsync(matchId, default)).ReturnsAsync(match);
        _gameServiceMock.Setup(s => s.CheckWinner(board)).Returns(GameResult.InProgress);

        await Assert.ThrowsAsync<DomainException>(() => _sut.HandleAsync(command));

        _matchRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Match>(), default), Times.Never);
    }

    [Fact]
    public async Task HandleAsync_WhenXWins_ReturnsMatchDtoWithPlayer1AsWinner()
    {
        var matchId = Guid.NewGuid();
        var board = new string?[] { "X", "X", "X", null, null, null, null, null, null };
        var command = new FinishMatchCommand(matchId, board);
        var match = new Match { Id = matchId, Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.InProgress, CreatedAt = DateTime.UtcNow };

        _matchRepositoryMock.Setup(r => r.GetByIdAsync(matchId, default)).ReturnsAsync(match);
        _gameServiceMock.Setup(s => s.CheckWinner(board)).Returns(GameResult.WinnerX);
        _matchServiceMock
            .Setup(s => s.Finish(match, GameResult.WinnerX, "Alice"))
            .Callback((Match m, GameResult r, string? w) => { m.Result = r; m.Winner = w; });

        var result = await _sut.HandleAsync(command);

        Assert.Equal(match.Id, result.Id);
        Assert.Equal(GameResult.WinnerX, result.Result);
        Assert.Equal("Alice", result.Winner);
        _matchServiceMock.Verify(s => s.Finish(match, GameResult.WinnerX, "Alice"), Times.Once);
        _matchRepositoryMock.Verify(r => r.UpdateAsync(match, default), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenOWins_ReturnsMatchDtoWithPlayer2AsWinner()
    {
        var matchId = Guid.NewGuid();
        var board = new string?[] { "O", "O", "O", null, null, null, null, null, null };
        var command = new FinishMatchCommand(matchId, board);
        var match = new Match { Id = matchId, Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.InProgress, CreatedAt = DateTime.UtcNow };

        _matchRepositoryMock.Setup(r => r.GetByIdAsync(matchId, default)).ReturnsAsync(match);
        _gameServiceMock.Setup(s => s.CheckWinner(board)).Returns(GameResult.WinnerO);
        _matchServiceMock
            .Setup(s => s.Finish(match, GameResult.WinnerO, "Bob"))
            .Callback((Match m, GameResult r, string? w) => { m.Result = r; m.Winner = w; });

        var result = await _sut.HandleAsync(command);

        Assert.Equal(match.Id, result.Id);
        Assert.Equal(GameResult.WinnerO, result.Result);
        Assert.Equal("Bob", result.Winner);
        _matchServiceMock.Verify(s => s.Finish(match, GameResult.WinnerO, "Bob"), Times.Once);
        _matchRepositoryMock.Verify(r => r.UpdateAsync(match, default), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenDraw_ReturnsMatchDtoWithNullWinner()
    {
        var matchId = Guid.NewGuid();
        var board = new string?[] { "X", "O", "X", "X", "X", "O", "O", "X", "O" };
        var command = new FinishMatchCommand(matchId, board);
        var match = new Match { Id = matchId, Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.InProgress, CreatedAt = DateTime.UtcNow };

        _matchRepositoryMock.Setup(r => r.GetByIdAsync(matchId, default)).ReturnsAsync(match);
        _gameServiceMock.Setup(s => s.CheckWinner(board)).Returns(GameResult.Draw);
        _matchServiceMock
            .Setup(s => s.Finish(match, GameResult.Draw, null))
            .Callback((Match m, GameResult r, string? w) => { m.Result = r; m.Winner = w; });

        var result = await _sut.HandleAsync(command);

        Assert.Equal(GameResult.Draw, result.Result);
        Assert.Null(result.Winner);
        _matchServiceMock.Verify(s => s.Finish(match, GameResult.Draw, null), Times.Once);
        _matchRepositoryMock.Verify(r => r.UpdateAsync(match, default), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithCancellationToken_PropagatesTokenToRepositories()
    {
        var matchId = Guid.NewGuid();
        var board = new string?[] { "X", "X", "X", null, null, null, null, null, null };
        var command = new FinishMatchCommand(matchId, board);
        var match = new Match { Id = matchId, Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.InProgress, CreatedAt = DateTime.UtcNow };
        using var cts = new CancellationTokenSource();

        _matchRepositoryMock.Setup(r => r.GetByIdAsync(matchId, cts.Token)).ReturnsAsync(match);
        _gameServiceMock.Setup(s => s.CheckWinner(board)).Returns(GameResult.WinnerX);

        await _sut.HandleAsync(command, cts.Token);

        _matchRepositoryMock.Verify(r => r.GetByIdAsync(matchId, cts.Token), Times.Once);
        _matchRepositoryMock.Verify(r => r.UpdateAsync(match, cts.Token), Times.Once);
    }
}
