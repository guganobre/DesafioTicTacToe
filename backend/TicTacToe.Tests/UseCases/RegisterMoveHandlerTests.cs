namespace TicTacToe.Tests.UseCases;

using Moq;
using TicTacToe.Application.UseCases.RegisterMove;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Exceptions;
using TicTacToe.Domain.Interfaces.Repositories;
using TicTacToe.Domain.Interfaces.Services;
using Match = TicTacToe.Domain.Entities.Match;
using Move = TicTacToe.Domain.Entities.Move;

public class RegisterMoveHandlerTests
{
    private readonly Mock<IMatchRepository> _matchRepositoryMock = new();
    private readonly Mock<IMoveRepository> _moveRepositoryMock = new();
    private readonly Mock<IMatchService> _matchServiceMock = new();
    private readonly RegisterMoveHandler _sut;

    public RegisterMoveHandlerTests()
    {
        _sut = new RegisterMoveHandler(
            _matchRepositoryMock.Object,
            _moveRepositoryMock.Object,
            _matchServiceMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WithValidCommand_ReturnsMoveDto()
    {
        var matchId = Guid.NewGuid();
        var command = new RegisterMoveCommand(matchId, PlayerSymbol.X, 4, 1);
        var match = new Match { Id = matchId, Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.InProgress, CreatedAt = DateTime.UtcNow };
        var move = new Move { Id = Guid.NewGuid(), MatchId = matchId, Player = PlayerSymbol.X, Position = 4, MoveOrder = 1, PlayedAt = DateTime.UtcNow };

        _matchRepositoryMock.Setup(r => r.GetByIdAsync(matchId, default)).ReturnsAsync(match);
        _matchServiceMock.Setup(s => s.CreateMove(matchId, PlayerSymbol.X, 4, 1)).Returns(move);

        var result = await _sut.HandleAsync(command);

        Assert.Equal(move.Id, result.Id);
        Assert.Equal(PlayerSymbol.X, result.Player);
        Assert.Equal(4, result.Position);
        Assert.Equal(1, result.MoveOrder);
        Assert.Equal(move.PlayedAt, result.PlayedAt);

        _moveRepositoryMock.Verify(r => r.AddAsync(move, default), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenMatchNotFound_ThrowsDomainException()
    {
        var matchId = Guid.NewGuid();
        var command = new RegisterMoveCommand(matchId, PlayerSymbol.X, 0, 1);

        _matchRepositoryMock.Setup(r => r.GetByIdAsync(matchId, default)).ReturnsAsync((Match?)null);

        await Assert.ThrowsAsync<DomainException>(() => _sut.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_WhenMatchFound_CallsMatchServiceCreateMoveWithCorrectArguments()
    {
        var matchId = Guid.NewGuid();
        var command = new RegisterMoveCommand(matchId, PlayerSymbol.O, 3, 2);
        var match = new Match { Id = matchId, Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.InProgress, CreatedAt = DateTime.UtcNow };
        var move = new Move { Id = Guid.NewGuid(), MatchId = matchId, Player = PlayerSymbol.O, Position = 3, MoveOrder = 2, PlayedAt = DateTime.UtcNow };

        _matchRepositoryMock.Setup(r => r.GetByIdAsync(matchId, default)).ReturnsAsync(match);
        _matchServiceMock.Setup(s => s.CreateMove(matchId, PlayerSymbol.O, 3, 2)).Returns(move);

        await _sut.HandleAsync(command);

        _matchServiceMock.Verify(s => s.CreateMove(matchId, PlayerSymbol.O, 3, 2), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithCancellationToken_PropagatesTokenToRepositories()
    {
        var matchId = Guid.NewGuid();
        var command = new RegisterMoveCommand(matchId, PlayerSymbol.X, 0, 1);
        var match = new Match { Id = matchId, Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.InProgress, CreatedAt = DateTime.UtcNow };
        var move = new Move { Id = Guid.NewGuid(), MatchId = matchId, Player = PlayerSymbol.X, Position = 0, MoveOrder = 1, PlayedAt = DateTime.UtcNow };
        using var cts = new CancellationTokenSource();

        _matchRepositoryMock.Setup(r => r.GetByIdAsync(matchId, cts.Token)).ReturnsAsync(match);
        _matchServiceMock.Setup(s => s.CreateMove(matchId, PlayerSymbol.X, 0, 1)).Returns(move);

        await _sut.HandleAsync(command, cts.Token);

        _matchRepositoryMock.Verify(r => r.GetByIdAsync(matchId, cts.Token), Times.Once);
        _moveRepositoryMock.Verify(r => r.AddAsync(move, cts.Token), Times.Once);
    }
}
