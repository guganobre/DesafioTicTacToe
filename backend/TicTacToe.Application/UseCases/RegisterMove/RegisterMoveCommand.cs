namespace TicTacToe.Application.UseCases.RegisterMove;

using TicTacToe.Domain.Enums;

public record RegisterMoveCommand(Guid MatchId, PlayerSymbol Player, int Position, int MoveOrder);
