namespace TicTacToe.Application.UseCases.FinishMatch;

using MediatR;
using TicTacToe.Application.DTOs;

public record FinishMatchCommand(Guid MatchId, string?[] Board) : IRequest<MatchDto>;
