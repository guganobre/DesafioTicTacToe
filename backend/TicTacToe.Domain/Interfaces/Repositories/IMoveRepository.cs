namespace TicTacToe.Domain.Interfaces.Repositories;

using TicTacToe.Domain.Entities;

public interface IMoveRepository
{
    Task<IEnumerable<Move>> GetByMatchIdAsync(Guid matchId, CancellationToken ct = default);
    Task AddAsync(Move move, CancellationToken ct = default);
}
