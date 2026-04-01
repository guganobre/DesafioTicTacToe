namespace TicTacToe.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Interfaces.Repositories;
using TicTacToe.Infrastructure.Persistence;

public class MoveRepository(AppDbContext context) : IMoveRepository
{
    public async Task<IEnumerable<Move>> GetByMatchIdAsync(Guid matchId, CancellationToken ct = default)
        => await context.Moves
            .Where(m => m.MatchId == matchId)
            .OrderBy(m => m.MoveOrder)
            .ToListAsync(ct);

    public async Task AddAsync(Move move, CancellationToken ct = default)
    {
        await context.Moves.AddAsync(move, ct);
        await context.SaveChangesAsync(ct);
    }
}
