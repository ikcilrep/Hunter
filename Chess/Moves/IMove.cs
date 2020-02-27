using System;
using Chess.Pieces;
using System.Linq;
using System.Text.RegularExpressions;

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

        public static IMove ParseMove(string moveString, Board board)
        {
            var isShortCastling = moveString.Equals("O-O");
            var isLongCastling = moveString.Equals("O-O-O");
            var color = !board.LastMove.Piece.Color;
            if (isShortCastling || isLongCastling)
            {
                var king = (King)board.Pieces.Values.Where(p => p.Color == color && p is King).First();
                if (Castling.IsCastling(king, isLongCastling, board))
                {
                    return new Castling(king, isLongCastling, board);
                }
            }
            var pawnPromotionRegex = new Regex(@"^([a-h]x)?[a-h][1-8]=[QBNRK]$");
            if (pawnPromotionRegex.IsMatch(moveString))
            {
                var isCapture = moveString[1] == 'x';
                var to = new Position(moveString.Substring(moveString.Length - 4, moveString.Length - 2));
                Position from;
                from = to.Behind(color);
                if (isCapture)
                {
                    from = new Position(from.Row, Position.ParseColumn(moveString[0]));
                }
                var piece = board.Pieces[from];
                Pawn pawn;
                if (piece is Pawn)
                {
                    pawn = (Pawn)piece;
                }
                else
                {
                    throw new ArgumentException();
                }
                var promotedPawn = Piece.ParsePiece(moveString[moveString.Length - 1], color);
                var promotion = new Promotion(pawn, promotedPawn, to, board);
                if (pawn.IsMovePossible(promotion, board))
                {
                    return promotion;
                }
                else
                {
                    throw new ArgumentException();
                }

            }
            throw new NotImplementedException();
        }

    }
}
