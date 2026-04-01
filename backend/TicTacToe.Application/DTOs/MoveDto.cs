namespace TicTacToe.Application.DTOs;

using TicTacToe.Domain.Enums;

public record MoveDto(
    Guid Id,
    PlayerSymbol Player,
    int Position,
    int MoveOrder,
    DateTime PlayedAt
);
