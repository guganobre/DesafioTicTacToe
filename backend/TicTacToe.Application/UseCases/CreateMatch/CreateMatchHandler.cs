namespace TicTacToe.Application.UseCases.CreateMatch;

using MediatR;
using TicTacToe.Application.DTOs;
using TicTacToe.Domain.Interfaces.Repositories;
using TicTacToe.Domain.Interfaces.Services;

public class CreateMatchHandler(IMatchRepository matchRepository, IMatchService matchService)
    : IRequestHandler<CreateMatchCommand, MatchDto>
{
    public async Task<MatchDto> Handle(CreateMatchCommand command, CancellationToken ct)
    {
        var match = matchService.Create(command.Player1Name, command.Player2Name);

        await matchRepository.AddAsync(match, ct);

        return new MatchDto(match.Id, match.Player1Name, match.Player2Name,
            match.Result, match.Winner, match.CreatedAt, []);
    }
}