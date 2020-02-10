using System;
using System.Collections.Generic;

namespace ChessMind
{
    public class King : Piece
    {
        public override bool IsMovePossible(Move move, Board board)
        {
            var position = board.FindPiece(this);
            var rowDistance = Math.Abs(position.Row - move.To.Row);
            var columnDistance = Math.Abs(position.Column - move.To.Column);
            var distancesAreRight = rowDistance
                   < 2 && columnDistance < 2 && !(columnDistance == 0 && rowDistance == 0);
            var thereIsAlliedPiece = board.IsTherePiece(position) && board[position].Color == Color;
            return  distancesAreRight && !thereIsAlliedPiece;     
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}