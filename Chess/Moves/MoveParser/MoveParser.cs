using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Moves
{
    public static class MoveParser
    {
        internal static Position GetToPosition(string moveString)
        {
            return new Position(moveString.Substring(moveString.Length - 2));
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
                return ParseChessmanMove(moveString, color, board);
            }
            throw new FormatException();
        }

        private static IEnumerable<Piece> SelectPiecesByPosition(string moveString, IEnumerable<Piece> pieces, Board board)
        {
            Func<Piece, bool> selectPiece;
            if (moveString[1] < 'a')
            {
                var row = Position.ParseRow(moveString[1]);
                selectPiece = p => board.FindPiece(p).Row == row;
            }
            else
            {
                var column = Position.ParseColumn(moveString[1]);
                selectPiece = p => board.FindPiece(p).Column == column;
            }

            return pieces.Where(selectPiece);
        }

        private static IMove ParseChessmanMove(string moveString, bool color, Board board)
        {
            var to = GetToPosition(moveString);
            var isPieceType = Piece.ParsePieceTypePredicate(moveString[0]);
            var pieces = board.Pieces.Values.Where(p => isPieceType(p) && p.Color == color && p.PossibleMoves(board).Count(m => m.To == to) > 0);
            if (moveString.Length > 3 && moveString[1] != 'x')
            {
                pieces = SelectPiecesByPosition(moveString, pieces, board);
            }

            if (pieces.Count() != 1)
            {
                throw new ArgumentException();
            }

            var piece = pieces.First();
            var move = new Move(piece, to, board);
            if (piece.IsMovePossible(move))
            {
                return move;
            }
            throw new ArgumentException();
        }
    }
}