using System;
using System.Linq;
using System.Collections.Generic;
using Chess.Moves;

namespace Chess.Pieces
{
    public class King : Piece
    {
        public King(bool color)
        {
            _color = color;
            _weight = 40;
        }
        public override bool IsMovePossible(IMove move, Board board)
        {
            if (move is Castling)
            {
                return true;
            }
            var position = board.FindPiece(this);
            var rowDistance = Math.Abs(position.Row - move.To.Row);
            var columnDistance = Math.Abs(position.Column - move.To.Column);
            var distancesAreRight = rowDistance
                   < 2 && columnDistance < 2;
            return distancesAreRight && move.To != position && !board.IsTherePieceOfColor(move.To, Color);
        }

        

        public override HashSet<IMove> PossibleMoves(Board board)
        {
            var position = board.FindPiece(this);

            var topRightCorner = new Position((byte)Math.Min(Position.MaxRow, position.Row + 1),
                                              (byte)Math.Min(Position.MaxColumn, position.Column + 1));

            var bottomLeftCorner = new Position((byte)Math.Max(Position.MinRow, position.Row - 1),
                                                (byte)Math.Max(Position.MinColumn, position.Column - 1));


            var range = Positions.Range(topRightCorner, bottomLeftCorner);
            range.ExceptWith(board.PiecesInRange(range, Color));

            var result = new HashSet<IMove>(range.Select(p => new Move(this, p, board)).ToHashSet());
            try
            {
                var to = new Position(position.Row, (byte)(position.Column + 2)); 
                if (Castling.IsCastling(this, to, board))
                {
                    result.Add(new Castling(this, to, board));
                }
            }
            catch { }

            try
            {
                var to = new Position(position.Row, (byte)(position.Column - 2));
                if (Castling.IsCastling(this, to ,board))
                {
                    result.Add(new Castling(this, to, board));
                }
            }
            catch { }

            return result;
        }
    }
}