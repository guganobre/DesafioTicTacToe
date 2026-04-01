namespace TicTacToe.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Domain.Interfaces.Repositories;
using TicTacToe.Infrastructure.Persistence;
using TicTacToe.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<IMoveRepository, MoveRepository>();

        return services;
    }
}