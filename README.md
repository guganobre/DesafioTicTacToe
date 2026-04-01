# Tic Tac Toe — Desafio Full Stack

Aplicação web completa do jogo da velha (Tic Tac Toe) com histórico de partidas e estatísticas, desenvolvida como desafio full stack.

---

## 🧱 Arquitetura

O backend segue os princípios de **Clean Architecture**, dividido em quatro camadas:

```
backend/
├── TicTacToe.Domain          # Entidades e regras de negócio
├── TicTacToe.Application     # Casos de uso e DTOs
├── TicTacToe.Infrastructure  # EF Core, repositórios e persistência
└── TicTacToe.API             # Controllers, configuração e entry point
```

---

## 🛠️ Tecnologias Utilizadas

### Backend
| Tecnologia | Versão |
|---|---|
| .NET / ASP.NET Core | 9.0 |
| Entity Framework Core | 9.0 |
| PostgreSQL (Npgsql) | 9.0 |
| Scalar (documentação de API) | 2.x |

### Frontend
| Tecnologia | Versão |
|---|---|
| React | 19 |
| Vite | 8 |
| React Router DOM | 7 |
| Axios | 1.x |
| Recharts | 3.x |
| Nginx (servidor em produção) | Alpine |

### Infraestrutura
| Tecnologia | Descrição |
|---|---|
| Docker | Containerização dos serviços |
| Docker Compose | Orquestração dos containers |
| PostgreSQL 17 Alpine | Banco de dados relacional |

---

## 🚀 Como Executar com Docker

### Pré-requisitos

- [Docker](https://www.docker.com/get-started) instalado e em execução
- [Docker Compose](https://docs.docker.com/compose/) (incluso no Docker Desktop)

### Passos

1. **Clone o repositório:**
   ```bash
   git clone <url-do-repositorio>
   cd DesafioTicTacToe
   ```

2. **Suba todos os serviços:**
   ```bash
   docker compose up --build
   ```
   > Na primeira execução, o Docker irá baixar as imagens base e construir os containers. As migrações do banco de dados são aplicadas automaticamente na inicialização da API.

3. **Acesse a aplicação:**

   | Serviço | URL |
   |---|---|
   | Frontend (React) | http://localhost:3000 |
   | API (ASP.NET Core) | http://localhost:5203 |
   | Documentação da API (Scalar) | http://localhost:5203/scalar/v1 |
   | PostgreSQL | `localhost:5432` |

### Encerrar os serviços

```bash
docker compose down
```

Para remover também o volume do banco de dados:

```bash
docker compose down -v
```

---

## 📡 Endpoints da API

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/matches` | Cria uma nova partida |
| `PUT` | `/api/matches/{id}/finish` | Finaliza uma partida com o estado do tabuleiro |
| `GET` | `/api/matches/last` | Retorna a última partida registrada |
| `GET` | `/api/matches` | Retorna o histórico de todas as partidas |

A documentação interativa completa está disponível em `/scalar/v1` quando a API está em execução.
