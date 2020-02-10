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

            if (rowDistance != 0)
            {
                var end = Math.Max(position.Row, move.To.Row);
                for (byte index = Math.Min(position.Row, move.To.Row); index <= end; index++)
                {
                    var newPosition = new Position((byte)(index + position.Row), position.Column);
                    if (board.IsTherePieceOfColor(newPosition, Color))
                    {
                        return false;
                    }
                }
            }
            else
            {
                var end = Math.Max(position.Column, move.To.Column);
                for (byte index = Math.Min(position.Column, move.To.Column); index <= end; index++)
                {
                    var newPosition = new Position(position.Row, (byte)(index + position.Column));
                    if (board.IsTherePieceOfColor(newPosition, Color))
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}