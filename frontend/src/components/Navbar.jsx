import { useState } from 'react';
import { NavLink } from 'react-router-dom';
import './Navbar.css';

export default function Navbar() {
    const [open, setOpen] = useState(false);

    const linkClass = ({ isActive }) =>
        isActive ? 'nav-link nav-link--active' : 'nav-link';

    return (
        <nav className="navbar">
            <div className="navbar-brand">
                <span className="brand-x">X</span>
                <span className="brand-name">Tic Tac Toe</span>
                <span className="brand-o">O</span>
            </div>

            {/* Hamburger button — visible only on small screens */}
            <button
                className={`navbar-hamburger${open ? ' navbar-hamburger--open' : ''}`}
                onClick={() => setOpen((v) => !v)}
                aria-label="Abrir menu"
                aria-expanded={open}
            >
                <span />
                <span />
                <span />
            </button>

            <div className={`navbar-links${open ? ' navbar-links--open' : ''}`}>
                <NavLink to="/" end className={linkClass} onClick={() => setOpen(false)}>
                    Jogar
                </NavLink>
                <NavLink to="/history" className={linkClass} onClick={() => setOpen(false)}>
                    Histórico
                </NavLink>
                <NavLink to="/stats" className={linkClass} onClick={() => setOpen(false)}>
                    Estatísticas
                </NavLink>
            </div>
        </nav>
    );
}
