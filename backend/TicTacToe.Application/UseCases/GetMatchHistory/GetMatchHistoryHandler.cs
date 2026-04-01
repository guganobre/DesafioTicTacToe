namespace TicTacToe.Application.UseCases.GetMatchHistory;

using TicTacToe.Application.DTOs;
using TicTacToe.Domain.Interfaces.Repositories;

public class GetMatchHistoryHandler(IMatchRepository matchRepository, IMoveRepository moveRepository)
{
    public async Task<IEnumerable<MatchDto>> HandleAsync(CancellationToken ct = default)
    {
        var matches = await matchRepository.GetAllAsync(ct);
        var result = new List<MatchDto>();

        foreach (var match in matches.OrderByDescending(m => m.CreatedAt))
        {
            var moves = await moveRepository.GetByMatchIdAsync(match.Id, ct);
            var moveDtos = moves.Select(m => new MoveDto(m.Id, m.Player, m.Position, m.MoveOrder, m.PlayedAt));
            result.Add(new MatchDto(match.Id, match.Player1Name, match.Player2Name,
                match.Result, match.Winner, match.CreatedAt, moveDtos));
        }

        return result;
    }
}
