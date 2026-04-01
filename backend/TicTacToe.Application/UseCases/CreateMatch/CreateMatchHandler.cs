namespace TicTacToe.Application.UseCases.CreateMatch;

using TicTacToe.Application.DTOs;
using TicTacToe.Domain.Interfaces.Repositories;
using TicTacToe.Domain.Interfaces.Services;

public class CreateMatchHandler(IMatchRepository matchRepository, IMatchService matchService)
{
    public async Task<MatchDto> HandleAsync(CreateMatchCommand command, CancellationToken ct = default)
    {
        var match = matchService.Create(command.Player1Name, command.Player2Name);

        await matchRepository.AddAsync(match, ct);

        return new MatchDto(match.Id, match.Player1Name, match.Player2Name,
            match.Result, match.Winner, match.CreatedAt, []);
    }
}