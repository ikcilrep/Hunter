using System;
using System.Linq;
using System.Collections.Generic;

namespace Chess

{
    public class King : Piece
    {
        public King(bool color)
        {
            _color = color;
            _weight = 40;
        }
        public override bool IsMovePossible(Move move, Board board)
        {
            if (move.IsCastling)
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

        internal static (Position, Rook) CastlingRook(Move move, Board board)
        {
            var kingPosition = board.FindPiece(move.Piece);
            var kvp = board.Pieces.Where(kvp => !board.HasPieceBeenMoved(kvp.Value)
                                                 && kvp.Key.Row == kingPosition.Row
                                                 && Math.Abs(move.To.Column - kvp.Key.Column) <= 2)
                                                 .First();
            return (kvp.Key, (Rook) kvp.Value); 
        }

        internal static bool IsCastling(Move move, Board board)
        {
            var kingPosition = board.FindPiece(move.Piece);
            if (board.HasPieceBeenMoved(move.Piece)
                || move.To.Row != kingPosition.Row
                || Math.Abs(move.To.Column - kingPosition.Column) != 2)
            {
                return false;
            }

            try
            {
                var (rookPosition, rook) = CastlingRook(move, board);
                var range = Positions.Range(kingPosition, rookPosition);
                var piecesInRange = board.Pieces.Count(kvp => range.Contains(kvp.Key));
                var possibleMovesToRange = board.PossibleMoves.Count(m => m.To != rookPosition
                                                         && range.Contains(m.To));
                return piecesInRange == 2 &&  possibleMovesToRange == 0;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            var position = board.FindPiece(this);

            var topRightCorner = new Position((byte)Math.Min(Position.MaxRow, position.Row + 1),
                                              (byte)Math.Min(Position.MaxColumn, position.Column + 1));

            var bottomLeftCorner = new Position((byte)Math.Max(Position.MinRow, position.Row - 1),
                                                (byte)Math.Max(Position.MinColumn, position.Column - 1));


            var range = Positions.Range(topRightCorner, bottomLeftCorner);
            range.ExceptWith(board.PiecesInRange(range, Color));

            var result = new HashSet<Move>(range.Select(p => new Move(this, p, board)).ToHashSet());
            try
            {
                var shortCastling = new Move(this, new Position(position.Row, (byte)(position.Column + 2)), board);
                if (shortCastling.IsCastling)
                {
                    result.Add(shortCastling);
                }
            }
            catch { }

            try
            {
                var longCastling = new Move(this, new Position(position.Row, (byte)(position.Column - 2)), board);
                if (longCastling.IsCastling)
                {
                    result.Add(longCastling);
                }
            }
            catch { }

            return result;
        }
    }
}