import axios from 'axios';

const api = axios.create({
    baseURL: '/api',
    headers: { 'Content-Type': 'application/json' },
});

export const createMatch = (player1Name, player2Name) =>
    api.post('/Matches', { player1Name, player2Name }).then((r) => r.data);

export const getMatches = (signal) =>
    api.get('/Matches', { signal }).then((r) => r.data);

export const getLastMatch = (signal) =>
    api.get('/Matches/last', { signal }).then((r) => r.data);

export const registerMove = (matchId, player, position, moveOrder) =>
    api
        .post(`/matches/${matchId}/Moves`, { matchId, player, position, moveOrder })
        .then((r) => r.data);

export const finishMatch = (matchId, boardState) =>
    api.put(`/Matches/${matchId}/finish`, boardState).then((r) => r.data);
