import './GameBoard.css';
import Square from './Square';

export default function GameBoard({
    board,
    match,
    currentPlayer,
    winnerInfo,
    isDraw,
    loading,
    gameOver,
    error,
    onMove,
    onReset,
    onRematch,
    onViewHistory,
}) {
    const player1 = match?.player1Name ?? 'Jogador 1';
    const player2 = match?.player2Name ?? 'Jogador 2';
    const currentName = currentPlayer === 0 ? player1 : player2;
    const currentSymbol = currentPlayer === 0 ? 'X' : 'O';

    let statusMessage;
    let statusClass = '';

    if (winnerInfo) {
        const winnerName = winnerInfo.symbol === 'X' ? player1 : player2;
        statusMessage = `🏆 ${winnerName} venceu!`;
        statusClass = 'status--win';
    } else if (isDraw) {
        statusMessage = '🤝 Empate!';
        statusClass = 'status--draw';
    } else {
        statusMessage = `Vez de ${currentName}`;
        statusClass = currentPlayer === 0 ? 'status--x' : 'status--o';
    }

    return (
        <div className="board-container">
            <div className="players-bar">
                <div className={`player-card ${currentPlayer === 0 && !gameOver ? 'player-card--active' : ''}`}>
                    <span className="player-symbol player-symbol--x">X</span>
                    <span className="player-name">{player1}</span>
                </div>
                <span className="board-vs">VS</span>
                <div className={`player-card ${currentPlayer === 1 && !gameOver ? 'player-card--active' : ''}`}>
                    <span className="player-symbol player-symbol--o">O</span>
                    <span className="player-name">{player2}</span>
                </div>
            </div>

            <div className={`status-bar ${statusClass}`}>
                {loading ? (
                    <span className="status-loading">
                        <span className="spinner" />
                        Registrando jogada...
                    </span>
                ) : (
                    statusMessage
                )}
            </div>

            {error && <div className="board-error">{error}</div>}

            <div className="board-grid">
                {board.map((value, idx) => (
                    <Square
                        key={idx}
                        value={value}
                        onClick={() => onMove(idx)}
                        isWinning={winnerInfo?.line.includes(idx)}
                        disabled={gameOver || loading}
                    />
                ))}
            </div>

            {gameOver && (
                <div className="board-actions">
                    <button className="btn btn--accent" onClick={onRematch} disabled={loading}>
                        Revanche
                    </button>
                    <button className="btn btn--primary" onClick={onReset}>
                        Nova Partida
                    </button>
                    <button className="btn btn--ghost" onClick={onViewHistory}>
                        Ver Histórico
                    </button>
                </div>
            )}
        </div>
    );
}
