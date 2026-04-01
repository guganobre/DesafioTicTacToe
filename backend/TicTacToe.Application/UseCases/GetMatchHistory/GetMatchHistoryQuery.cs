namespace TicTacToe.Application.UseCases.GetMatchHistory;

using MediatR;
using TicTacToe.Application.DTOs;

public record GetMatchHistoryQuery : IRequest<IEnumerable<MatchDto>>;
