using System;
using System.Linq;
using System.Collections.Generic;

namespace Chess

{
    public class King : Piece
    {
        public King(bool color) {
            _color = color;
            _weight = 40; 
        }
        public override bool IsMovePossible(Move move, Board board)
        {
            var position = board.FindPiece(this);
            var rowDistance = Math.Abs(position.Row - move.To.Row);
            var columnDistance = Math.Abs(position.Column - move.To.Column);
            var distancesAreRight = rowDistance
                   < 2 && columnDistance < 2;
            return distancesAreRight && move.To != position && !board.IsTherePieceOfColor(move.To, Color) ;
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            var position = board.FindPiece(this);

            var topRightCorner = new Position((byte)Math.Min(Position.MaxRow, position.Row + 1),
                                              (byte)Math.Min(Position.MaxColumn, position.Column + 1)); 

            var bottomLeftCorner = new Position((byte)Math.Max(Position.MinRow, position.Row - 1),
                                                (byte)Math.Max(Position.MinColumn, position.Column - 1));


            var range = Position.Range(topRightCorner, bottomLeftCorner);
            range.ExceptWith(board.PiecesInRange(range, Color));
            
            return range.Select(p => new Move(this, p, board)).ToHashSet();
        }
    }
}