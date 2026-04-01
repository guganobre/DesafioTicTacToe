namespace TicTacToe.Application.UseCases.GetLastMatch;

using MediatR;
using TicTacToe.Application.DTOs;
using TicTacToe.Domain.Interfaces.Repositories;

public class GetLastMatchHandler(IMatchRepository matchRepository)
    : IRequestHandler<GetLastMatchQuery, MatchDto?>
{
    public async Task<MatchDto?> Handle(GetLastMatchQuery query, CancellationToken ct)
    {
        var match = await matchRepository.GetLastAsync(ct);

        if (match is null)
            return null;

        var moveDtos = match.Moves
            .OrderBy(m => m.MoveOrder)
            .Select(m => new MoveDto(m.Id, m.Player, m.Position, m.MoveOrder, m.PlayedAt));

        return new MatchDto(match.Id, match.Player1Name, match.Player2Name,
            match.Result, match.Winner, match.CreatedAt, moveDtos);
    }
}
