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

        private static Promotion ParsePromotion(string promotionString, bool color, Board board)
        {
            var isCapture = promotionString[1] == 'x';
            var to = new Position(promotionString.Substring(promotionString.Length - 4, promotionString.Length - 2));
            Position from;
            from = to.Behind(color);
            if (isCapture)
            {
                from = new Position(from.Row, Position.ParseColumn(promotionString[0]));
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
            var promotedPawn = Piece.ParsePiece(promotionString[promotionString.Length - 1], color);
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
                return ParsePromotion(moveString, color, board);
            }
            throw new NotImplementedException();
        }

    }
}
