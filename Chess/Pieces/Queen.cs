using System.Collections.Generic;

namespace Chess

{
    public class Queen : Piece
    {
        public Queen(bool color) 
        {
            _color = color;
            _weight = 9;
        }

        public override bool IsMovePossible(Move move, Board board)
        {
            return Bishop.IsMovePossibleStatic(move, board) || Rook.IsMovePossibleStatic(move, board);
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            var moves = Bishop.PossibleMovesStatic(this, board);
            moves.UnionWith(Rook.PossibleMovesStatic(this, board));
            return moves;
        }
    }
}