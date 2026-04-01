namespace TicTacToe.Application.DTOs;

public record PlayerStatsDto(
    string PlayerName,
    int Wins,
    int Draws,
    int Losses,
    int TotalMatches
);
