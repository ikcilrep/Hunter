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
            Init();
        }

        public override bool IsMovePossible(IMove move)
        {
            return Bishop.IsMovePossibleStatic(move) || Rook.IsMovePossibleStatic(move);
        }

        public override HashSet<IMove> PossibleMoves(Board board)
        {
            var moves = Bishop.PossibleMovesStatic(this, board);
            moves.UnionWith(Rook.PossibleMovesStatic(this, board));
            return moves;
        }

        public override string ToString()
        {
            return "Q";
        }
    }

}
