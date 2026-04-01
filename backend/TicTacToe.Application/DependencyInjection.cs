namespace TicTacToe.Application;

using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Application.Services;
using TicTacToe.Application.UseCases.CreateMatch;
using TicTacToe.Application.UseCases.FinishMatch;
using TicTacToe.Application.UseCases.GetLastMatch;
using TicTacToe.Application.UseCases.GetMatchHistory;
using TicTacToe.Application.UseCases.RegisterMove;
using TicTacToe.Domain.Interfaces.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IMatchService, MatchService>();
        services.AddScoped<CreateMatchHandler>();
        services.AddScoped<FinishMatchHandler>();
        services.AddScoped<GetMatchHistoryHandler>();
        services.AddScoped<GetLastMatchHandler>();
        services.AddScoped<RegisterMoveHandler>();

        return services;
    }
}