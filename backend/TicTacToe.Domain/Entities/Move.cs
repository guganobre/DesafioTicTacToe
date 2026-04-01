namespace TicTacToe.Domain.Entities;

using TicTacToe.Domain.Enums;

public class Move
{
    public Guid Id { get; set; }
    public Guid MatchId { get; set; }
    public PlayerSymbol Player { get; set; }
    public int Position { get; set; }
    public int MoveOrder { get; set; }
    public DateTime PlayedAt { get; set; }
}