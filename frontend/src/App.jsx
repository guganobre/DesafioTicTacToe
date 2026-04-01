import { useEffect, useState } from 'react';
import { BrowserRouter, Route, Routes, useNavigate } from 'react-router-dom';
import './App.css';
import GameBoard from './components/GameBoard';
import MatchHistory from './components/MatchHistory';
import Navbar from './components/Navbar';
import PlayerSetup from './components/PlayerSetup';
import ResumeModal from './components/ResumeModal';
import SamePlayersModal from './components/SamePlayersModal';
import Statistics from './components/Statistics';
import { useGame } from './hooks/useGame';
import { getLastMatch } from './services/api';

function GamePage() {
  const navigate = useNavigate();
  const {
    match,
    board,
    currentPlayer,
    winnerInfo,
    isDraw,
    loading,
    gameOver,
    error,
    startGame,
    makeMove,
    resetGame,
    rematch,
    resumeMatch,
  } = useGame();

  const [pendingMatch, setPendingMatch] = useState(undefined); // undefined=checking, null=none, obj=found (in-progress)
  const [lastMatch, setLastMatch] = useState(null);             // última partida concluída
  const [showSamePlayers, setShowSamePlayers] = useState(false);

  useEffect(() => {
    const controller = new AbortController();
    getLastMatch(controller.signal)
      .then((last) => {
        if (last && last.result === 0) {
          // Em Andamento → perguntar se quer continuar
          setPendingMatch(last);
        } else if (last) {
          // Concluída → perguntar se quer usar mesmos jogadores
          setLastMatch(last);
          setShowSamePlayers(true);
          setPendingMatch(null);
        } else {
          setPendingMatch(null);
        }
      })
      .catch((err) => {
        if (!controller.signal.aborted) setPendingMatch(null);
      });
    return () => controller.abort();
  }, []);

  // Ainda verificando
  if (pendingMatch === undefined) return null;

  // Modal: retomar partida em andamento
  if (pendingMatch !== null && !match) {
    return (
      <ResumeModal
        match={pendingMatch}
        onResume={() => { resumeMatch(pendingMatch); setPendingMatch(null); }}
        onDecline={() => setPendingMatch(null)}
      />
    );
  }

  // Modal: usar mesmos jogadores da última partida concluída
  if (showSamePlayers && !match) {
    return (
      <SamePlayersModal
        match={lastMatch}
        onConfirm={() => { setShowSamePlayers(false); startGame(lastMatch.player1Name, lastMatch.player2Name); }}
        onDecline={() => setShowSamePlayers(false)}
      />
    );
  }

  if (!match) {
    return <PlayerSetup onStart={startGame} loading={loading} />;
  }

  return (
    <GameBoard
      board={board}
      match={match}
      currentPlayer={currentPlayer}
      winnerInfo={winnerInfo}
      isDraw={isDraw}
      loading={loading}
      gameOver={gameOver}
      error={error}
      onMove={makeMove}
      onReset={resetGame}
      onRematch={rematch}
      onViewHistory={() => navigate('/history')}
    />
  );
}

function HistoryPage() {
  return (
    <div className="page">
      <h2 className="page-title">Histórico de Partidas</h2>
      <MatchHistory />
    </div>
  );
}

function StatsPage() {
  return (
    <div className="page">
      <h2 className="page-title">Estatísticas</h2>
      <Statistics />
    </div>
  );
}

function App() {
  return (
    <BrowserRouter>
      <div className="app-layout">
        <Navbar />
        <main className="app-main">
          <Routes>
            <Route path="/" element={<GamePage />} />
            <Route path="/history" element={<HistoryPage />} />
            <Route path="/stats" element={<StatsPage />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}

export default App;
