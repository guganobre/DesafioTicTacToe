import { useEffect, useState } from 'react';
import {
    Bar,
    BarChart,
    Cell,
    Legend,
    Pie,
    PieChart,
    ResponsiveContainer,
    Tooltip,
    XAxis,
    YAxis,
} from 'recharts';
import { getMatches } from '../services/api';
import './Statistics.css';

// GameResult enum: 0=InProgress, 1=Player1Wins, 2=Player2Wins, 3=Draw
function buildStats(matches) {
    const players = new Set();
    const wins = {};
    let draws = 0;
    let total = 0;

    for (const m of matches) {
        players.add(m.player1Name);
        players.add(m.player2Name);

        if (m.result === 0) continue; // pula partidas em andamento

        total++;
        if (m.result === 3) {
            draws++;
        } else if (m.winner) {
            // result === 1 (P1 vence) ou result === 2 (P2 vence)
            wins[m.winner] = (wins[m.winner] ?? 0) + 1;
        }
    }

    // Todos os jogadores aparecem no gráfico de barras, mesmo com 0 vitórias
    const barData = [...players]
        .map((name) => ({ name, vitórias: wins[name] ?? 0 }))
        .sort((a, b) => b.vitórias - a.vitórias);

    const pieData = [
        ...Object.entries(wins).map(([name, value]) => ({ name, value })),
        ...(draws > 0 ? [{ name: 'Empates', value: draws }] : []),
    ];

    return { barData, pieData, total, draws };
}

const COLORS = ['#63b3ed', '#fc814a', '#68d391', '#f6e05e', '#b794f4', '#76e4f7'];

export default function Statistics() {
    const [matches, setMatches] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const controller = new AbortController();
        getMatches(controller.signal)
            .then(setMatches)
            .catch((err) => {
                if (!controller.signal.aborted)
                    setError('Não foi possível carregar estatísticas.');
            })
            .finally(() => {
                if (!controller.signal.aborted) setLoading(false);
            });
        return () => controller.abort();
    }, []);

    if (loading) {
        return (
            <div className="stats-state">
                <div className="spinner-lg" />
                <p>Carregando estatísticas...</p>
            </div>
        );
    }

    if (error) {
        return <div className="stats-state stats-state--error">{error}</div>;
    }

    const { barData, pieData, total, draws } = buildStats(matches);

    if (total === 0) {
        return (
            <div className="stats-state">
                <p>Nenhuma partida concluída ainda.</p>
            </div>
        );
    }

    return (
        <div className="stats-container">
            <div className="stats-cards">
                <div className="stat-card">
                    <span className="stat-value">{total}</span>
                    <span className="stat-label">Partidas</span>
                </div>
                <div className="stat-card">
                    <span className="stat-value">{draws}</span>
                    <span className="stat-label">Empates</span>
                </div>
                <div className="stat-card">
                    <span className="stat-value">
                        {barData[0]?.name ?? '—'}
                    </span>
                    <span className="stat-label">Líder do Ranking</span>
                </div>
            </div>

            <div className="charts-grid">
                <div className="chart-card">
                    <h3 className="chart-title">Vitórias por Jogador</h3>
                    <ResponsiveContainer width="100%" height={250}>
                        <BarChart data={barData} margin={{ top: 10, right: 20, left: 0, bottom: 0 }}>
                            <XAxis dataKey="name" tick={{ fill: 'var(--text-muted)', fontSize: 12 }} />
                            <YAxis allowDecimals={false} tick={{ fill: 'var(--text-muted)', fontSize: 12 }} />
                            <Tooltip
                                contentStyle={{
                                    background: 'var(--card-bg)',
                                    border: '1px solid var(--border)',
                                    borderRadius: 8,
                                    color: 'var(--text)',
                                }}
                            />
                            <Bar dataKey="vitórias" radius={[6, 6, 0, 0]}>
                                {barData.map((_, idx) => (
                                    <Cell key={idx} fill={COLORS[idx % COLORS.length]} />
                                ))}
                            </Bar>
                        </BarChart>
                    </ResponsiveContainer>
                </div>

                <div className="chart-card">
                    <h3 className="chart-title">Distribuição de Resultados</h3>
                    <ResponsiveContainer width="100%" height={250}>
                        <PieChart>
                            <Pie
                                data={pieData}
                                cx="50%"
                                cy="50%"
                                innerRadius={60}
                                outerRadius={95}
                                paddingAngle={3}
                                dataKey="value"
                                label={({ name, percent }) =>
                                    `${name} ${(percent * 100).toFixed(0)}%`
                                }
                                labelLine={false}
                            >
                                {pieData.map((_, idx) => (
                                    <Cell key={idx} fill={COLORS[idx % COLORS.length]} />
                                ))}
                            </Pie>
                            <Legend
                                formatter={(value) => (
                                    <span style={{ color: 'var(--text)', fontSize: '0.85rem' }}>{value}</span>
                                )}
                            />
                            <Tooltip
                                contentStyle={{
                                    background: 'var(--card-bg)',
                                    border: '1px solid var(--border)',
                                    borderRadius: 8,
                                    color: 'var(--text)',
                                }}
                            />
                        </PieChart>
                    </ResponsiveContainer>
                </div>
            </div>
        </div>
    );
}
