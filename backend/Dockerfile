FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY TicTacToe.API/TicTacToe.API.csproj TicTacToe.API/
COPY TicTacToe.Application/TicTacToe.Application.csproj TicTacToe.Application/
COPY TicTacToe.Infrastructure/TicTacToe.Infrastructure.csproj TicTacToe.Infrastructure/
COPY TicTacToe.Domain/TicTacToe.Domain.csproj TicTacToe.Domain/
RUN dotnet restore TicTacToe.API/TicTacToe.API.csproj

COPY . .
RUN dotnet publish TicTacToe.API/TicTacToe.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TicTacToe.API.dll"]
