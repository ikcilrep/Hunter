using System.Collections.Generic;
using Chess.Moves;

namespace Chess.Pieces
{
    public class Queen : Piece
    {
        public Queen(bool color)
        {
            _color = color;
            _weight = 9;
        }

        public override bool IsMovePossible(IMove move, Board board)
        {
            return Bishop.IsMovePossibleStatic(move, board) || Rook.IsMovePossibleStatic(move, board);
        }

        public override HashSet<IMove> PossibleMoves(Board board)
        {
            var moves = Bishop.PossibleMovesStatic(this, board);
            moves.UnionWith(Rook.PossibleMovesStatic(this, board));
            return moves;
        }
    }
}