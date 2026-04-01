import { useState } from 'react';
import './PlayerSetup.css';

export default function PlayerSetup({ onStart, loading }) {
    const [player1, setPlayer1] = useState('');
    const [player2, setPlayer2] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        const p1 = player1.trim();
        const p2 = player2.trim();
        if (p1 && p2 && p1 !== p2) {
            onStart(p1, p2);
        }
    };

    const valid =
        player1.trim().length > 0 &&
        player2.trim().length > 0 &&
        player1.trim() !== player2.trim();

    return (
        <div className="setup-container">
            <div className="setup-card">
                <div className="setup-icon">
                    <span className="x-symbol">X</span>
                    <span className="vs-label">VS</span>
                    <span className="o-symbol">O</span>
                </div>
                <h1 className="setup-title">Jogo da Velha</h1>
                <p className="setup-subtitle">Insira os nomes dos jogadores para começar</p>

                <form className="setup-form" onSubmit={handleSubmit}>
                    <div className="input-group">
                        <label htmlFor="player1">
                            <span className="player-badge player-badge--x">X</span>
                            Jogador 1
                        </label>
                        <input
                            id="player1"
                            type="text"
                            placeholder="Nome do Jogador 1"
                            value={player1}
                            onChange={(e) => setPlayer1(e.target.value)}
                            maxLength={30}
                            autoFocus
                        />
                    </div>

                    <div className="input-group">
                        <label htmlFor="player2">
                            <span className="player-badge player-badge--o">O</span>
                            Jogador 2
                        </label>
                        <input
                            id="player2"
                            type="text"
                            placeholder="Nome do Jogador 2"
                            value={player2}
                            onChange={(e) => setPlayer2(e.target.value)}
                            maxLength={30}
                        />
                    </div>

                    {player1.trim() && player2.trim() && player1.trim() === player2.trim() && (
                        <p className="validation-error">Os nomes devem ser diferentes.</p>
                    )}

                    <button
                        type="submit"
                        className="btn-start"
                        disabled={!valid || loading}
                    >
                        {loading ? 'Iniciando...' : 'Iniciar Partida'}
                    </button>
                </form>
            </div>
        </div>
    );
}
