namespace TicTacToe.Application.UseCases.GetLastMatch;

using MediatR;
using TicTacToe.Application.DTOs;

public record GetLastMatchQuery : IRequest<MatchDto?>;
