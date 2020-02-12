using System.Collections.Generic;

namespace Chess
{
    public class Bishop : Piece
    {
        public override bool IsMovePossible(Move move, Board board)
        {
            try 
            {
                var position = board.FindPiece(this);
                var diagonal = Position.Diagonal(position, move.To);

                foreach (var diagonalPosition in diagonal) { 
                    if (board.IsTherePiece(diagonalPosition)) 
                    {
                        return false;
                    }
                }
                return true;
            } 
            catch 
            {
                return false;
            }
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}