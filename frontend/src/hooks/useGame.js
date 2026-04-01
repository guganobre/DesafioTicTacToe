import { useCallback, useState } from 'react';
import * as api from '../services/api';

const WINNING_LINES = [
    [0, 1, 2], [3, 4, 5], [6, 7, 8],
    [0, 3, 6], [1, 4, 7], [2, 5, 8],
    [0, 4, 8], [2, 4, 6],
];

export function checkWinner(board) {
    for (const [a, b, c] of WINNING_LINES) {
        if (board[a] && board[a] === board[b] && board[a] === board[c]) {
            return { symbol: board[a], line: [a, b, c] };
        }
    }
    return null;
}

export function useGame() {
    const [match, setMatch] = useState(null);
    const [board, setBoard] = useState(Array(9).fill(null));
    const [firstPlayer, setFirstPlayer] = useState(0); // quem iniciou esta partida
    const [currentPlayer, setCurrentPlayer] = useState(0); // 0 = X (P1), 1 = O (P2)
    const [moveOrder, setMoveOrder] = useState(1);
    const [winnerInfo, setWinnerInfo] = useState(null); // { symbol, line }
    const [isDraw, setIsDraw] = useState(false);
    const [loading, setLoading] = useState(false);
    const [gameOver, setGameOver] = useState(false);
    const [error, setError] = useState(null);

    const startGame = useCallback(async (player1Name, player2Name, first = 0) => {
        setError(null);
        setLoading(true);
        try {
            const newMatch = await api.createMatch(player1Name, player2Name);
            setMatch(newMatch);
            setBoard(Array(9).fill(null));
            setFirstPlayer(first);
            setCurrentPlayer(first);
            setMoveOrder(1);
            setWinnerInfo(null);
            setIsDraw(false);
            setGameOver(false);
        } catch (err) {
            setError('Falha ao criar partida. Verifique se o backend está rodando.');
            throw err;
        } finally {
            setLoading(false);
        }
    }, []);

    const makeMove = useCallback(
        async (position) => {
            if (!match || board[position] !== null || gameOver || loading) return;

            const symbol = currentPlayer === 0 ? 'X' : 'O';
            const newBoard = [...board];
            newBoard[position] = symbol;
            setBoard(newBoard);
            setLoading(true);
            setError(null);

            try {
                await api.registerMove(match.id, currentPlayer, position, moveOrder);

                const winner = checkWinner(newBoard);
                const draw = !winner && newBoard.every((cell) => cell !== null);

                if (winner || draw) {
                    const boardState = newBoard.map((c) => c ?? '');
                    await api.finishMatch(match.id, boardState);
                    setGameOver(true);
                    if (winner) setWinnerInfo(winner);
                    if (draw) setIsDraw(true);
                } else {
                    setCurrentPlayer((prev) => (prev === 0 ? 1 : 0));
                    setMoveOrder((prev) => prev + 1);
                }
            } catch (err) {
                // Revert the board on error
                setBoard(board);
                setError('Erro ao registrar jogada. Tente novamente.');
            } finally {
                setLoading(false);
            }
        },
        [match, board, currentPlayer, moveOrder, gameOver, loading]
    );

    const resetGame = useCallback(() => {
        setMatch(null);
        setBoard(Array(9).fill(null));
        setCurrentPlayer(0);
        setMoveOrder(1);
        setWinnerInfo(null);
        setIsDraw(false);
        setGameOver(false);
        setError(null);
    }, []);

    const rematch = useCallback(async () => {
        if (!match) return;
        // Inverte quem iniciou a partida anterior
        await startGame(match.player1Name, match.player2Name, firstPlayer === 0 ? 1 : 0);
    }, [match, firstPlayer, startGame]);

    // Retoma uma partida existente, reconstruindo o tabuleiro a partir dos moves
    const resumeMatch = useCallback((existingMatch) => {
        const moves = [...(existingMatch.moves ?? [])].sort((a, b) => a.moveOrder - b.moveOrder);
        const restoredBoard = Array(9).fill(null);
        for (const mv of moves) {
            restoredBoard[mv.position] = mv.player === 0 ? 'X' : 'O';
        }
        // Próximo jogador: X começa (first=0), alterna a cada move
        const next = moves.length % 2 === 0 ? 0 : 1;
        setMatch(existingMatch);
        setBoard(restoredBoard);
        setFirstPlayer(0);
        setCurrentPlayer(next);
        setMoveOrder(moves.length + 1);
        setWinnerInfo(null);
        setIsDraw(false);
        setGameOver(false);
        setError(null);
    }, []);

    return {
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
    };
}
