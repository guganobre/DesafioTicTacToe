namespace TicTacToe.Application;

using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Application.Services;
using TicTacToe.Domain.Interfaces.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IMatchService, MatchService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GameService>());

        return services;
    }
}