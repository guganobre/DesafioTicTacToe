namespace TicTacToe.Domain.Interfaces.Repositories;

using TicTacToe.Domain.Entities;

public interface IMatchRepository
{
    Task<Match?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Match?> GetLastAsync(CancellationToken ct = default);
    Task<IEnumerable<Match>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Match match, CancellationToken ct = default);
    Task UpdateAsync(Match match, CancellationToken ct = default);
}
