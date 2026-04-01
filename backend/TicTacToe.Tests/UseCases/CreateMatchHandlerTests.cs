namespace TicTacToe.Tests.UseCases;

using Moq;
using TicTacToe.Application.UseCases.CreateMatch;
using TicTacToe.Domain.Enums;
using TicTacToe.Domain.Interfaces.Repositories;
using TicTacToe.Domain.Interfaces.Services;
using Match = TicTacToe.Domain.Entities.Match;

public class CreateMatchHandlerTests
{
    private readonly Mock<IMatchRepository> _matchRepositoryMock = new();
    private readonly Mock<IMatchService> _matchServiceMock = new();
    private readonly CreateMatchHandler _sut;

    public CreateMatchHandlerTests()
    {
        _sut = new CreateMatchHandler(_matchRepositoryMock.Object, _matchServiceMock.Object);
    }

    [Fact]
    public async Task HandleAsync_WithValidCommand_ReturnsMatchDto()
    {
        var command = new CreateMatchCommand("Alice", "Bob");
        var match = new Match
        {
            Id = Guid.NewGuid(),
            Player1Name = "Alice",
            Player2Name = "Bob",
            Result = GameResult.InProgress,
            CreatedAt = DateTime.UtcNow
        };

        _matchServiceMock
            .Setup(s => s.Create(command.Player1Name, command.Player2Name))
            .Returns(match);

        var result = await _sut.Handle(command, default);

        Assert.Equal(match.Id, result.Id);
        Assert.Equal("Alice", result.Player1Name);
        Assert.Equal("Bob", result.Player2Name);
        Assert.Equal(GameResult.InProgress, result.Result);
        Assert.Equal(match.CreatedAt, result.CreatedAt);
        Assert.Null(result.Winner);

        _matchRepositoryMock.Verify(r => r.AddAsync(match, default), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_CallsMatchServiceCreate()
    {
        var command = new CreateMatchCommand("Alice", "Bob");
        var match = new Match { Id = Guid.NewGuid(), Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.InProgress, CreatedAt = DateTime.UtcNow };

        _matchServiceMock.Setup(s => s.Create("Alice", "Bob")).Returns(match);

        await _sut.Handle(command, default);

        _matchServiceMock.Verify(s => s.Create("Alice", "Bob"), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WithCancellationToken_PropagatesTokenToRepository()
    {
        var command = new CreateMatchCommand("Alice", "Bob");
        var match = new Match { Id = Guid.NewGuid(), Player1Name = "Alice", Player2Name = "Bob", Result = GameResult.InProgress, CreatedAt = DateTime.UtcNow };
        using var cts = new CancellationTokenSource();

        _matchServiceMock.Setup(s => s.Create("Alice", "Bob")).Returns(match);

        await _sut.Handle(command, cts.Token);

        _matchRepositoryMock.Verify(r => r.AddAsync(match, cts.Token), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_WhenServiceThrowsArgumentException_PropagatesException()
    {
        var command = new CreateMatchCommand("", "Bob");

        _matchServiceMock
            .Setup(s => s.Create(command.Player1Name, command.Player2Name))
            .Throws<ArgumentException>();

        await Assert.ThrowsAsync<ArgumentException>(() => _sut.Handle(command, default));
    }
}
