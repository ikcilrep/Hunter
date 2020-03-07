using System;
using System.Linq;
using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Moves
{
    public static class ChessmenMoveParser
    {
        internal static Castling ParseCastling(bool isLong, bool color, Board board)
        {
            var king = (King)board.Pieces.Values.Where(p => p.Color == color && p is King).First();
            if (Castling.IsCastling(king, isLong, board))
            {
                return new Castling(king, isLong, board);
            }
            throw new ArgumentException();
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

        private static IEnumerable<Piece> PiecesThatCanBeMovedTo(Position to, Func<Piece, bool> predicate, Board board)
        {
            return board.Pieces.Values.Where(p => predicate(p) && p.Color == board.CurrentMoveColor && p.PossibleMoves(board).Any(m => m.To == to));
        }

        internal static IMove ParseChessmanMove(string moveString, bool color, Board board)
        {
            var to = MoveParser.GetToPosition(moveString);
            var isPieceType = PieceParser.ParsePieceTypePredicate(moveString[0]);
            var pieces = PiecesThatCanBeMovedTo(to, isPieceType, board);
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