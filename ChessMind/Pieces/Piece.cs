using System.Collections.Generic;
using System.Linq;

namespace ChessMind
{
    public abstract class Piece
    {
        public byte Weight { get => _weight; }
        public bool Color { get => _color; }
        internal byte _weight; 
        internal bool _color; 

        public abstract HashSet<Move> PossibleMoves(Board board);

        public virtual HashSet<Move> PossibleCaptures(Board board) {
            return PossibleMoves(board).Where(x => x.IsCapture).ToHashSet();
        }

        public abstract bool IsMovePossible(Move move, Board board);

    }
}