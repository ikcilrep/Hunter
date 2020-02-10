using System.Collections.Generic;
using System.Linq;

namespace ChessMind
{
    public abstract class Piece
    {
        public byte Weight { get; }
        public bool Color { get; }

        public abstract HashSet<Move> PossibleMoves(Board board);

        public virtual HashSet<Move> PossibleCaptures(Board board) {
            return PossibleMoves(board).Where(x => x.IsCapture).ToHashSet();
        }

        public abstract bool IsMovePossible(Move move, Board board);

    }
}