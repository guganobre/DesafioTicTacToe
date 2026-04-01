import './ResumeModal.css';

export default function SamePlayersModal({ match, onConfirm, onDecline }) {
    return (
        <div className="modal-overlay">
            <div className="modal-card">
                <div className="modal-icon">👥</div>
                <h2 className="modal-title">Mesmos jogadores?</h2>
                <p className="modal-desc">
                    Deseja iniciar uma nova partida com os mesmos jogadores?
                </p>
                <div className="modal-players-preview">
                    <span className="preview-player preview-player--x">
                        <strong>X</strong> {match.player1Name}
                    </span>
                    <span className="preview-vs">vs</span>
                    <span className="preview-player preview-player--o">
                        <strong>O</strong> {match.player2Name}
                    </span>
                </div>
                <div className="modal-actions">
                    <button className="btn-modal btn-modal--primary" onClick={onConfirm}>
                        Sim, usar mesmos jogadores
                    </button>
                    <button className="btn-modal btn-modal--ghost" onClick={onDecline}>
                        Não, inserir novos jogadores
                    </button>
                </div>
            </div>
        </div>
    );
}
