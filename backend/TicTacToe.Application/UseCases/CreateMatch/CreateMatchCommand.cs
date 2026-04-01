namespace TicTacToe.Application.UseCases.CreateMatch;

using MediatR;
using TicTacToe.Application.DTOs;

public record CreateMatchCommand(string Player1Name, string Player2Name) : IRequest<MatchDto>;