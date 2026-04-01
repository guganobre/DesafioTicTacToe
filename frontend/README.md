# Tic Tac Toe — Frontend

Interface web do jogo da velha, desenvolvida em **React 19** com **Vite**. Consome a API REST do backend para persistir partidas e exibir histórico e estatísticas.

---

## 🛠️ Tecnologias

| Tecnologia | Versão | Uso |
|---|---|---|
| [React](https://react.dev/) | 19 | Biblioteca principal de UI |
| [Vite](https://vite.dev/) | 8 | Build tool com HMR e servidor de desenvolvimento |
| [React Router DOM](https://reactrouter.com/) | 7 | Roteamento client-side entre páginas |
| [Axios](https://axios-http.com/) | 1.x | Cliente HTTP para comunicação com a API |
| [Recharts](https://recharts.org/) | 3.x | Gráficos de barras e pizza nas estatísticas |
| [Nginx](https://nginx.org/) | Alpine | Servidor web para servir o build em produção |

---

## 🗂️ Estrutura

```
src/
├── components/
│   ├── GameBoard        # Tabuleiro principal do jogo
│   ├── Square           # Célula individual do tabuleiro
│   ├── PlayerSetup      # Formulário de nomes dos jogadores
│   ├── Navbar           # Barra de navegação entre páginas
│   ├── MatchHistory     # Tabela do histórico de partidas
│   ├── Statistics       # Gráficos de vitórias e resultados
│   ├── ResumeModal      # Modal para retomar partida em andamento
│   └── SamePlayersModal # Modal de revanche com os mesmos jogadores
├── hooks/
│   └── useGame.js       # Hook com toda a lógica e estado do jogo
├── services/
│   └── api.js           # Cliente Axios com as chamadas à API REST
├── App.jsx              # Componente raiz com as rotas definidas
└── main.jsx             # Entry point da aplicação
```

---

## ✨ Funcionalidades

- **Iniciar partida** — cadastro dos nomes dos dois jogadores antes de jogar
- **Tabuleiro interativo** — detecção de vitória e empate em tempo real, com destaque na linha vencedora
- **Retomar partida** — ao carregar a página, verifica se existe uma partida em andamento e oferece a opção de continuar
- **Revanche** — ao final de uma partida, oferece jogar novamente com os mesmos jogadores ou alternar quem começa
- **Histórico** — listagem de todas as partidas ordenadas da mais recente para a mais antiga
- **Estatísticas** — gráfico de barras com vitórias por jogador e gráfico de pizza com a distribuição de resultados

---

## ⚙️ Configuração

O Vite está configurado com **proxy** para o backend, eliminando problemas de CORS em desenvolvimento:

```js
// vite.config.js
server: {
  proxy: {
    '/api': {
      target: 'http://localhost:5203',
      changeOrigin: true,
    },
  },
},
```

Todas as chamadas para `/api/*` são redirecionadas automaticamente para a API em `http://localhost:5203`.

---

## 🚀 Executar em Desenvolvimento

```bash
npm install
npm run dev
```

Acesse **http://localhost:5173** — a API deve estar rodando em `http://localhost:5203`.

## 📦 Build de Produção

```bash
npm run build   # gera os arquivos estáticos em /dist
npm run preview # pré-visualiza o build localmente
```
