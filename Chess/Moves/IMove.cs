using System;
using Chess.Pieces;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Chess.Moves
{
    public interface IMove
    {
        public Piece Piece { get; }
        public Position To { get; }
        public Board Board { get; }

        public static bool IsCapture(IMove move) => (move is Promotion && ((Promotion)move).IsCapture)
            || (move is Move && ((Move)move).IsCapture)
            || move is EnPassant;

        private static (Pawn, Position) GetPawnAndToPosition(string pawnMoveString, bool color, Board board)
        {
            var isCapture = pawnMoveString[1] == 'x';
            var to = new Position(pawnMoveString.Substring(pawnMoveString.Length - 4, pawnMoveString.Length - 2));
            Position from;
            from = to.Behind(color);
            if (isCapture)
            {
                from = new Position(from.Row, Position.ParseColumn(pawnMoveString[0]));
            }
            Piece piece;
            try
            {
                piece = board.Pieces[from];
            }
            catch (KeyNotFoundException)
            {
                throw new ArgumentException();
            }

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
            if (pawn.IsMovePossible(promotion, board))
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
            if (pawn.IsMovePossible(move, board))
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

            /* var chessmanMoveRegex = new Regex(@"^[QBRNK]x?[a-h][1-8]$");
            if (chessmanMoveRegex.IsMatch(moveString))
            {
                return ParseChessmanMove(moveString, color, board);
            }*/
            throw new NotImplementedException();
        }

    }
}
