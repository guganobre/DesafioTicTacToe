namespace TicTacToe.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Interfaces.Repositories;
using TicTacToe.Infrastructure.Persistence;

public class MatchRepository(AppDbContext context) : IMatchRepository
{
    public async Task<Match?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await context.Matches
            .Include(m => m.Moves)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<Match?> GetLastAsync(CancellationToken cancellationToken = default)
        => await context.Matches
            .Include(m => m.Moves)
            .OrderByDescending(m => m.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IEnumerable<Match>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.Matches
            .Include(m => m.Moves)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Match match, CancellationToken cancellationToken = default)
    {
        await context.Matches.AddAsync(match, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Match match, CancellationToken cancellationToken = default)
    {
        context.Matches.Update(match);
        await context.SaveChangesAsync(cancellationToken);
    }
}