import './ResumeModal.css';

export default function ResumeModal({ match, onResume, onDecline }) {
    return (
        <div className="modal-overlay">
            <div className="modal-card">
                <div className="modal-icon">⏸️</div>
                <h2 className="modal-title">Partida em andamento</h2>
                <p className="modal-desc">
                    Foi encontrada uma partida inacabada entre
                    <strong> {match.player1Name}</strong> e
                    <strong> {match.player2Name}</strong>.
                </p>
                <p className="modal-moves">
                    {match.moves?.length ?? 0} jogada(s) realizadas
                </p>
                <div className="modal-actions">
                    <button className="btn-modal btn-modal--primary" onClick={onResume}>
                        Continuar partida
                    </button>
                    <button className="btn-modal btn-modal--ghost" onClick={onDecline}>
                        Nova partida
                    </button>
                </div>
            </div>
        </div>
    );
}
