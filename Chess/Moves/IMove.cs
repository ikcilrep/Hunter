using System;
using Chess.Pieces;

namespace Chess.Moves
{
    public interface IMove
    {
        public Piece Piece { get; }
        public Position To { get; }

        public static bool isCapture(IMove move) => (move is Move && ((Move)move).IsCapture)
                || move is EnPassant;
    }
}
