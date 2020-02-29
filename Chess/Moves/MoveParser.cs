using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Moves
{
    public static class MoveParser
    {
        private static Position GetToPosition(string moveString)
        {
            return new Position(moveString.Substring(moveString.Length - 2));
        }

        private static Position GetPawnPosition(string pawnMoveString, Position to, bool color)
        {
            var from = to.Behind(color);
            var isCapture = pawnMoveString[1] == 'x';
            if (isCapture)
            {
                from = new Position(from.Row, Position.ParseColumn(pawnMoveString[0]));
            }
            return from;

        }

        private static Piece GetPieceAtPosition(Board board, Position position) {
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
            var to = GetToPosition(pawnMoveString);
            var from = GetPawnPosition(pawnMoveString, to, color);
            var piece = GetPieceAtPosition(board, from);
            if (piece.Color != color || piece is Pawn)
            {
                return ((Pawn)piece, to);
            }
            throw new ArgumentException();
        }

        private static Promotion ParsePromotion(string promotionString, bool color, Board board)
        {
            (Pawn pawn, Position to) = GetPawnAndToPosition(promotionString.Substring(0, promotionString.Length - 2), color, board);
            var promotedPawn = Piece.ParsePiece(promotionString[promotionString.Length - 1], color);
            var promotion = new Promotion(pawn, promotedPawn, to, board);
            if (pawn.IsMovePossible(promotion))
            {
                return promotion;
            }
            throw new ArgumentException();
        }

        private static IMove ParsePawnMove(string moveString, bool color, Board board)
        {
            (Pawn pawn, Position to) = GetPawnAndToPosition(moveString, color, board);
            if (EnPassant.IsEnPassant(pawn, to, board))
            {
                return new EnPassant(pawn, to, board);
            }
            var move = new Move(pawn, to, board);
            if (pawn.IsMovePossible(move))
            {
                return move;
            }

            throw new ArgumentException();
        }

        private static Castling ParseCastling(bool isLong, bool color, Board board)
        {
            var king = (King)board.Pieces.Values.Where(p => p.Color == color && p is King).First();
            if (Castling.IsCastling(king, isLong, board))
            {
                return new Castling(king, isLong, board);
            }
            throw new ArgumentException();
        }

        public static IMove ParseMove(string moveString, Board board)
        {
            var isShortCastling = moveString.Equals("O-O");
            var isLongCastling = moveString.Equals("O-O-O");
            var color = !board.LastMove.Piece.Color;
            if (isShortCastling || isLongCastling)
            {
                return ParseCastling(isLongCastling, color, board);
            }

            var pawnPromotionRegex = new Regex(@"^([a-h]x)?[a-h][1-8]=[QBNRK]$");
            if (pawnPromotionRegex.IsMatch(moveString))
            {
                return ParsePromotion(moveString, color, board);
            }

            var pawnMoveRegex = new Regex(@"^([a-h]x)?[a-h][1-8]$");
            if (pawnMoveRegex.IsMatch(moveString))
            {
                return ParsePromotion(moveString, color, board);
            }

            /*var chessmanMoveRegex = new Regex(@"^[QBRNK]x?[a-h][1-8]$");
            if (chessmanMoveRegex.IsMatch(moveString))
            {
                return ParseChessmanMove(moveString, color, board);
            }*/
            throw new NotImplementedException();
        }
    }
}