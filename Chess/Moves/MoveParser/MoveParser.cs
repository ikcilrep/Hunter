using System;
using System.Text.RegularExpressions;

namespace Chess.Moves
{
    public static class MoveParser
    {
        internal static Position GetToPosition(string moveString)
        {
            return new Position(moveString.Substring(moveString.Length - 2));
        }


        public static IMove ParseMove(string moveString, Board board)
        {
            var isShortCastling = moveString.Equals("O-O");
            var isLongCastling = moveString.Equals("O-O-O");
            var color = board.CurrentMoveColor;
            if (isShortCastling || isLongCastling)
            {
                return ChessmenMoveParser.ParseCastling(isLongCastling, color, board);
            }

            var pawnPromotionRegex = new Regex(@"^([a-h]x)?[a-h][1-8]=[QBNR]$");
            if (pawnPromotionRegex.IsMatch(moveString))
            {
                return PawnMoveParser.ParsePromotion(moveString, color, board);
            }

            var pawnMoveRegex = new Regex(@"^([a-h]x)?[a-h][1-8]$");
            if (pawnMoveRegex.IsMatch(moveString))
            {
                return PawnMoveParser.ParsePawnMove(moveString, color, board);
            }

            var chessmanMoveRegex = new Regex(@"^[QBRNK]([a-h]|[0-9])?x?[a-h][1-8]$");
            if (chessmanMoveRegex.IsMatch(moveString))
            {
                return ChessmenMoveParser.ParseChessmanMove(moveString, color, board);
            }
            throw new FormatException();
        }

        
    }
}