using System;
using System.Collections;
using System.Collections.Generic;

namespace ChessMind
{
    public class Rook : Piece
    {
        public override bool IsMovePossible(Move move, Board board)
        {
            var position = board.FindPiece(this);
            var columnDistance = move.To.Column - position.Column;
            var rowDistance = move.To.Row - position.Row;
            var movesStraight = columnDistance == 0 ^ rowDistance == 0;
            if (!movesStraight)
            {
                return false;
            }
           return board.PiecesInRanges(move.To, position, Color).Count > 0;
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}