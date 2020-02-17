using System;
using Chess.Pieces;

namespace Chess.Moves
{
    public interface IMove
    {
        public Piece Piece { get; }
        public Position To { get; }

        public static bool IsCapture(IMove move) => (move is Promotion && ((Promotion)move).IsCapture) || (move is Move && ((Move)move).IsCapture)
                || move is EnPassant;

    }
}
