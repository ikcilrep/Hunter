using System;
using System.Linq;
using Chess.Pieces;

namespace Chess.Moves
{
    public class Castling : IMove
    {

        public Piece Piece => King;

        public Position To { get; }

        public King King { get; }

        public Rook Rook { get; }

        public Position RookPosition { get; }

        public Board Board { get; }

        private bool _isLong;

        public Position RookTo
        {
            get
            {

                var rookToKingDistance = Math.Abs(To.Column - RookPosition.Column);
                return RookPosition.GoInDirectionOf(
                        To.Column,
                        (byte)(rookToKingDistance + 1));

            }
        }

        public Castling(King king, bool isLong, Board board)
        {
            King = king;
            (RookPosition, Rook) = CastlingRook(King, To, board);
            _isLong = isLong;
            Board = board;
            To = GetToPosition(Board.FindPiece(King), _isLong);
        }

        public override string ToString()
        {
            if (_isLong)
            {
                return "O-O-O";
            }
            return "O-O";
        }


        public static bool IsCastling(King king, bool isLongCastling, Board board)
        {
            var kingPosition = board.FindPiece(king);
            Position to;
            try
            {
                to = GetToPosition(kingPosition, isLongCastling);
            }
            catch (ArgumentException)
            {
                return false;
            }
            if (board.HasPieceBeenMoved(king)
                || to.Row != kingPosition.Row
                || Math.Abs(to.Column - kingPosition.Column) != 2)
            {
                return false;
            }

            try
            {
                var (rookPosition, rook) = CastlingRook(king, to, board);
                var range = Positions.Range(kingPosition, rookPosition);
                var piecesInRange = board.Pieces.Count(kvp => range.Contains(kvp.Key));
                return piecesInRange == 2;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public static (Position, Rook) CastlingRook(King king, Position to, Board board)
        {
            var kingPosition = board.FindPiece(king);
            var kvp = board.Pieces.Where(kvp => kvp.Value is Rook
                                                 && !board.HasPieceBeenMoved(kvp.Value)
                                                 && kvp.Key.Row == kingPosition.Row
                                                 && Math.Abs(to.Column - kvp.Key.Column) <= 2)
                                                 .First();
            return (kvp.Key, (Rook)kvp.Value);
        }

        public static Position GetToPosition(Position kingPosition, bool isLongCastling)
        {
            if (isLongCastling)
            {
                return new Position(kingPosition.Row, (byte)(kingPosition.Column - 2));
            }
            return new Position(kingPosition.Row, (byte)(kingPosition.Column + 2));
        }

        public override bool Equals(object obj)
        {
            return obj is Castling castling && castling._isLong == _isLong && castling.King == King && castling.Board == Board;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(_isLong, King, Board).GetHashCode();
        }
    }
}
