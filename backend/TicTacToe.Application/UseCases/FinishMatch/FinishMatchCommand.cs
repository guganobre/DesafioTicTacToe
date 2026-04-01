namespace TicTacToe.Application.UseCases.FinishMatch;

public record FinishMatchCommand(Guid MatchId, string?[] Board);
