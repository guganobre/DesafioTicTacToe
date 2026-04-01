import { useEffect, useState } from 'react';
import { getMatches } from '../services/api';
import './MatchHistory.css';

// GameResult enum: 0=InProgress, 1=Player1Wins, 2=Player2Wins, 3=Draw
const RESULT_LABELS = {
    0: { label: 'Em Andamento', cls: 'result--progress' },
    1: { label: 'Vitória J1', cls: 'result--p1' },
    2: { label: 'Vitória J2', cls: 'result--p2' },
    3: { label: 'Empate', cls: 'result--draw' },
};

function formatDate(iso) {
    return new Date(iso).toLocaleString('pt-BR', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
    });
}

export default function MatchHistory() {
    const [matches, setMatches] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const controller = new AbortController();
        getMatches(controller.signal)
            .then((data) =>
                setMatches(
                    data.slice().sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))
                )
            )
            .catch((err) => {
                if (!controller.signal.aborted)
                    setError('Não foi possível carregar o histórico.');
            })
            .finally(() => {
                if (!controller.signal.aborted) setLoading(false);
            });
        return () => controller.abort();
    }, []);

    if (loading) {
        return (
            <div className="history-state">
                <div className="spinner-lg" />
                <p>Carregando histórico...</p>
            </div>
        );
    }

    if (error) {
        return <div className="history-state history-state--error">{error}</div>;
    }

    if (matches.length === 0) {
        return (
            <div className="history-state">
                <p className="history-empty">Nenhuma partida registrada ainda.</p>
            </div>
        );
    }

    return (
        <div className="history-container">
            <table className="history-table">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Jogador X</th>
                        <th>Jogador O</th>
                        <th>Resultado</th>
                        <th>Vencedor</th>
                        <th>Jogadas</th>
                        <th>Data</th>
                    </tr>
                </thead>
                <tbody>
                    {matches.map((m, idx) => {
                        const res = RESULT_LABELS[m.result] ?? { label: '—', cls: '' };
                        return (
                            <tr key={m.id}>
                                <td className="td-num">{matches.length - idx}</td>
                                <td>
                                    <span className="player-tag player-tag--x">X</span>
                                    {m.player1Name}
                                </td>
                                <td>
                                    <span className="player-tag player-tag--o">O</span>
                                    {m.player2Name}
                                </td>
                                <td>
                                    <span className={`result-badge ${res.cls}`}>{res.label}</span>
                                </td>
                                <td>{(m.result === 1 || m.result === 2) && m.winner ? m.winner : '—'}</td>
                                <td>{m.moves?.length ?? 0}</td>
                                <td className="td-date">{formatDate(m.createdAt)}</td>
                            </tr>
                        );
                    })}
                </tbody>
            </table>
        </div>
    );
}
