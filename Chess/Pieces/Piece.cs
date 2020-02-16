using System.Collections.Generic;
using System.Linq;
using Chess.Moves;

namespace Chess

{
    public abstract class Piece
    {
        public byte Weight { get => _weight; }
        public bool Color { get => _color; }
        internal byte _weight; 
        internal bool _color; 

        public abstract HashSet<IMove> PossibleMoves(Board board);

        public virtual HashSet<IMove> PossibleCaptures(Board board) {
            return PossibleMoves(board).Where(m => m is Move && ((Move)m).IsCapture).ToHashSet();
        }

        public abstract bool IsMovePossible(IMove move, Board board);

    }
}