﻿using System;
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

        public Castling(King king, Position to, Board board)
        {
            King = king;
            (RookPosition, Rook) = CastlingRook(King, To, board);
            To = to;
            _isLong = Math.Abs(To.Column - RookPosition.Column) == 2;
        }

        public override string ToString()
        {
            if (_isLong)
            {
                return "O-O-O";
            }
            return "O-O";
        }


        public static bool IsCastling(King king, Position to, Board board)
        {
            var kingPosition = board.FindPiece(king);
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
                var possibleMovesToRange = board.PossibleMoves.Count(m => m.To != rookPosition
                                                         && range.Contains(m.To));
                return piecesInRange == 2 && possibleMovesToRange == 0;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public static (Position, Rook) CastlingRook(King king, Position to, Board board)
        {
            var kingPosition = board.FindPiece(king);
            var kvp = board.Pieces.Where(kvp => !board.HasPieceBeenMoved(kvp.Value)
                                                 && kvp.Key.Row == kingPosition.Row
                                                 && Math.Abs(to.Column - kvp.Key.Column) <= 2)
                                                 .First();
            return (kvp.Key, (Rook)kvp.Value);
        }
    }
}
