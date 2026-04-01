# Tic Tac Toe — Backend

API REST desenvolvida em **ASP.NET Core 9** seguindo os princípios de **Clean Architecture**. Responsável por persistir partidas, registrar jogadas e determinar o resultado do jogo.

---

## 🛠️ Tecnologias

| Tecnologia | Versão | Uso |
|---|---|---|
| [.NET / ASP.NET Core](https://dotnet.microsoft.com/) | 9.0 | Framework principal da API |
| [Entity Framework Core](https://learn.microsoft.com/ef/core/) | 9.0 | ORM para acesso ao banco de dados |
| [Npgsql.EF Core](https://www.npgsql.org/efcore/) | 9.0 | Provider PostgreSQL para o EF Core |
| [Scalar](https://scalar.com/) | 2.x | Interface de documentação interativa da API |
| [Microsoft.AspNetCore.OpenApi](https://learn.microsoft.com/aspnet/core/fundamentals/openapi/) | 9.0 | Geração do schema OpenAPI |

---

## 🧱 Arquitetura

O projeto é organizado em quatro camadas com dependências unidirecionais:

```
TicTacToe.Domain          ← núcleo, sem dependências externas
TicTacToe.Application     ← depende de Domain
TicTacToe.Infrastructure  ← depende de Domain e Application
TicTacToe.API             ← depende de Application e Infrastructure
```

### TicTacToe.Domain
Contém as regras e contratos de negócio, sem nenhuma dependência de framework.

| Artefato | Descrição |
|---|---|
| `Entities/Match` | Entidade de partida com jogadores, resultado e coleção de jogadas |
| `Entities/Move` | Entidade de jogada com posição, símbolo do jogador e ordem |
| `Enums/GameResult` | `InProgress`, `WinnerX`, `WinnerO`, `Draw` |
| `Enums/PlayerSymbol` | Símbolo do jogador (`X` ou `O`) |
| `Interfaces/Repositories` | Contratos `IMatchRepository` e `IMoveRepository` |
| `Interfaces/Services` | Contratos `IGameService` e `IMatchService` |
| `Exceptions/DomainException` | Exceção base para violações de regra de negócio |

### TicTacToe.Application
Orquestra os casos de uso, implementa os serviços de domínio e define os DTOs de resposta.

| Artefato | Descrição |
|---|---|
| `UseCases/CreateMatch` | Cria uma nova partida com dois jogadores |
| `UseCases/RegisterMove` | Registra uma jogada em uma partida em andamento |
| `UseCases/FinishMatch` | Finaliza uma partida e determina o resultado |
| `UseCases/GetLastMatch` | Retorna a última partida registrada |
| `UseCases/GetMatchHistory` | Retorna o histórico completo de partidas |
| `Services/GameService` | Verifica vencedor e estado do tabuleiro (8 combinações vencedoras) |
| `Services/MatchService` | Lógica de negócio relacionada à partida |
| `DTOs/MatchDto` | Representação de saída de uma partida |
| `DTOs/MoveDto` | Representação de saída de uma jogada |

### TicTacToe.Infrastructure
Implementa os repositórios e o contexto de banco de dados com EF Core.

| Artefato | Descrição |
|---|---|
| `Persistence/AppDbContext` | Contexto EF Core com os DbSets de `Match` e `Move` |
| `Persistence/Configurations` | Fluent API para mapeamento de `Match` e `Move` |
| `Repositories/MatchRepository` | Implementação de `IMatchRepository` |
| `Repositories/MoveRepository` | Implementação de `IMoveRepository` |
| `Migrations/` | Migrações geradas pelo EF Core |

### TicTacToe.API
Entry point da aplicação. Configura os serviços, middlewares e expõe os controllers.

| Artefato | Descrição |
|---|---|
| `Controllers/MatchesController` | Endpoints de gerenciamento de partidas |
| `Controllers/MovesController` | Endpoint de registro de jogadas |
| `Program.cs` | Configuração da aplicação e aplicação automática de migrações |

---

## 📡 Endpoints

### Partidas — `/api/matches`

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/matches` | Cria uma nova partida |
| `GET` | `/api/matches` | Lista o histórico de todas as partidas |
| `GET` | `/api/matches/last` | Retorna a última partida registrada |
| `PUT` | `/api/matches/{id}/finish` | Finaliza uma partida com o estado final do tabuleiro |

### Jogadas — `/api/matches/{matchId}/moves`

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/matches/{matchId}/moves` | Registra uma jogada em uma partida |

A documentação interativa completa está disponível em **`/scalar/v1`** quando a API está em execução.

---

## ⚙️ Connection String

Definida em `appsettings.json` (padrão) e sobrescrita por `appsettings.Development.json` em ambiente de desenvolvimento:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=tictactoe;Username=postgres;Password=senha123"
}
```

Ajuste os valores de `Password` (e demais campos) conforme a sua instalação local do PostgreSQL.

---

## 🚀 Executar sem Docker

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- PostgreSQL rodando localmente **ou** via Docker (somente o banco):

```bash
# Somente o banco de dados
docker compose up postgres -d
```

### 1. Restaurar dependências

```bash
cd backend
dotnet restore
```

### 2. Ajustar a connection string *(se necessário)*

Edite `TicTacToe.API/appsettings.Development.json` com as credenciais do seu PostgreSQL local:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=tictactoe;Username=postgres;Password=<sua-senha>"
}
```

### 3. Executar a API

**Via terminal:**
```bash
dotnet run --project TicTacToe.API
```

**Via Visual Studio:**

Abra `TicTacToe.sln`, selecione o perfil `http` e pressione `F5`.

> As migrações do banco de dados são aplicadas automaticamente na inicialização.

### 4. Acessar

| Serviço | URL |
|---|---|
| API (HTTP) | http://localhost:5203 |
| API (HTTPS) | https://localhost:7170 |
| Documentação Scalar | http://localhost:5203/scalar/v1 |

---

## 🗃️ Migrações (EF Core)

Para criar uma nova migration após alterar as entidades:

```bash
cd backend
dotnet ef migrations add <NomeDaMigration> --project TicTacToe.Infrastructure --startup-project TicTacToe.API
```

Para aplicar manualmente as migrations pendentes:

```bash
dotnet ef database update --project TicTacToe.Infrastructure --startup-project TicTacToe.API
```
