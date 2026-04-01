namespace TicTacToe.Domain.Entities;

using TicTacToe.Domain.Enums;

public class Match
{
    public Guid Id { get; set; }
    public string Player1Name { get; set; } = string.Empty;
    public string Player2Name { get; set; } = string.Empty;
    public GameResult Result { get; set; }
    public string? Winner { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Move> Moves { get; set; } = [];
}