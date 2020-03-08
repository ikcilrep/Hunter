using System;
using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Moves
{
    public static class PawnMoveParser
    {
        private static Position GetPawnPosition(string pawnMoveString, Position to, bool color, Board board)
        {
            var from = to.Behind(color);
            var isCapture = pawnMoveString[1] == 'x';
            if (isCapture)
            {
                from = new Position(from.Row, Position.ParseColumn(pawnMoveString[0]));
            }
            else if (!board.IsTherePiece(from))
            {
                from = from.Behind(color);
            }


            return from;

        }

        private static Piece GetPieceAtPosition(Board board, Position position)
        {
            try
            {
                return board.Pieces[position];
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException();
            }

        }

        private static (Pawn, Position) GetPawnAndToPosition(string pawnMoveString, bool color, Board board)
        {
            var to = MoveParser.GetToPosition(pawnMoveString);
            var from = GetPawnPosition(pawnMoveString, to, color, board);
            var piece = GetPieceAtPosition(board, from);
            if (piece.Color != color || piece is Pawn)
            {
                return ((Pawn)piece, to);
            }
            throw new ArgumentException();
        }

        internal static Promotion ParsePromotion(string promotionString, bool color, Board board)
        {
            (Pawn pawn, Position to) = GetPawnAndToPosition(promotionString.Substring(0, promotionString.Length - 2), color, board);
            var promotedPawn = PieceParser.ParsePiece(promotionString[promotionString.Length - 1], color);
            var promotion = new Promotion(pawn, promotedPawn, to, board);
            if (pawn.IsMovePossible(promotion))
            {
                return promotion;
            }
            throw new ArgumentException();
        }

        internal static IMove ParsePawnMove(string moveString, bool color, Board board)
        {
            (Pawn pawn, Position to) = GetPawnAndToPosition(moveString, color, board);
            var isCapture = moveString[1] == 'x';
            if (isCapture == board.IsTherePieceOfColor(to, !board.CurrentMoveColor))
            {
                if (EnPassant.IsEnPassant(pawn, to, board))
                {
                    return new EnPassant(pawn, to, board);
                }
                var move = new Move(pawn, to, board, isCapture);
                if (pawn.IsMovePossible(move))
                {
                    return move;
                }
            }
            throw new ArgumentException();
        }

    }
}