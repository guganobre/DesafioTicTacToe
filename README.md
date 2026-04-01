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

### Ferramentas de Desenvolvimento
| Ferramenta | Descrição |
|---|---|
| Visual Studio 2026 | IDE principal para desenvolvimento do backend (requer VS 2022+) |

---

## 🚀 Como Executar com Docker

### Pré-requisitos

- [Docker](https://www.docker.com/get-started) instalado e em execução
- [Docker Compose](https://docs.docker.com/compose/) (incluso no Docker Desktop)

### Passos

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/guganobre/DesafioTicTacToe
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

## 🐛 Executando em Modo Debug (sem Docker)

Para desenvolvimento local, é necessário ter o PostgreSQL disponível. A forma mais prática é subir **apenas o banco** via Docker e rodar a API e o frontend direto na máquina.

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- [Node.js 22+](https://nodejs.org/)
- Docker (apenas para o banco de dados)

### 1. Subir somente o banco de dados

```bash
docker compose up postgres -d
```

### 2. Executar a API

**Via terminal:**
```bash
cd backend
dotnet run --project TicTacToe.API
```

**Via Visual Studio:**

Abra `backend/TicTacToe.sln`, selecione o perfil `http` e pressione `F5`.

A API estará disponível em:

| Serviço | URL |
|---|---|
| API (HTTP) | http://localhost:5203 |
| API (HTTPS) | https://localhost:7170 |
| Documentação Scalar | http://localhost:5203/scalar/v1 |

### 3. Executar o Frontend

```bash
cd frontend
npm install
npm run dev
```

O frontend estará disponível em **http://localhost:5173**.

> O Vite já está configurado com proxy: requisições para `/api` são redirecionadas automaticamente para `http://localhost:5203`, sem necessidade de alterar nenhuma configuração.

---

## 📡 Endpoints da API

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/matches` | Cria uma nova partida |
| `PUT` | `/api/matches/{id}/finish` | Finaliza uma partida com o estado do tabuleiro |
| `GET` | `/api/matches/last` | Retorna a última partida registrada |
| `GET` | `/api/matches` | Retorna o histórico de todas as partidas |

A documentação interativa completa está disponível em `/scalar/v1` quando a API está em execução.
