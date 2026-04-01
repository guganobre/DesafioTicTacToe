namespace TicTacToe.Application.DTOs;

using TicTacToe.Domain.Enums;

public record MatchDto(
    Guid Id,
    string Player1Name,
    string Player2Name,
    GameResult Result,
    string? Winner,
    DateTime CreatedAt,
    IEnumerable<MoveDto> Moves
);
