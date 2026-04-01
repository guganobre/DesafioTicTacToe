export default function Square({ value, onClick, isWinning, disabled }) {
    const cls = [
        'square',
        value === 'X' ? 'square--x' : value === 'O' ? 'square--o' : '',
        isWinning ? 'square--winning' : '',
        !value && !disabled ? 'square--empty' : '',
    ]
        .filter(Boolean)
        .join(' ');

    return (
        <button className={cls} onClick={onClick} disabled={disabled || !!value}>
            {value && <span className="square-symbol">{value}</span>}
        </button>
    );
}
