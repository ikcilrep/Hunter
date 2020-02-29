using System.Collections.Generic;
using System.Linq;
using Chess.Moves;

namespace Chess.Pieces
{
    public abstract class Piece
    {
        public byte Weight { get => _weight; }
        public bool Color { get => _color; }
        internal byte _weight;
        internal bool _color;

        public abstract HashSet<IMove> PossibleMoves(Board board);

        public virtual HashSet<IMove> PossibleCaptures(Board board)
        {
            return PossibleMoves(board).Where(m => m is Move && ((Move)m).IsCapture).ToHashSet();
        }

        public abstract bool IsMovePossible(IMove move);

        public static Piece ParsePiece(char symbol, bool color)
        {
            switch (symbol)
            {
                case 'B':
                    return new Bishop(color);
                case 'K':
                    return new King(color);
                case 'N':
                    return new Knight(color);
                case 'Q':
                    return new Queen(color);
                case 'R':
                    return new Rook(color);
                default:
                    throw new System.FormatException();
            }
        }

    }
}