namespace TicTacToe.Application.UseCases.RegisterMove;

using MediatR;
using TicTacToe.Application.DTOs;
using TicTacToe.Domain.Enums;

public record RegisterMoveCommand(Guid MatchId, PlayerSymbol Player, int Position, int MoveOrder) : IRequest<MoveDto>;
