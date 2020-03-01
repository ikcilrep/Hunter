using System;
using Chess.Pieces;

namespace Chess.Moves
{
    public class Promotion : IMove
    {
        public Piece Piece => Pawn;
        public Pawn Pawn { get; }

        public Piece PromotedPawn { get; }

        public Position To { get; }

        public bool IsCapture { get; }

        public Board Board { get; }

        public Promotion(Pawn pawn, Piece promotedPawn, Position to, Board board)
        {
            Pawn = pawn;
            PromotedPawn = promotedPawn;
            To = to;
            IsCapture = board.IsTherePieceOfColor(To, !Piece.Color);
            Board = board;
        }

        public Promotion(Pawn pawn, Piece promotedPawn, Position to, Board board, bool isCapture)
        {
            Pawn = pawn;
            To = to;
            IsCapture = isCapture;
            Board = board;
        }

        public override string ToString()
        {
            var result = "";
            if (IsCapture)
            {
                result += Board.FindPiece(Pawn).ToString()[0] + "x";
            }
            result += To + "=" + PromotedPawn.ToString();
            return result;
        }

        public override bool Equals(object obj)
        {
            return obj is Promotion promotion
                   && promotion.Pawn == Pawn
                   && promotion.To == To
                   && promotion.PromotedPawn == promotion.PromotedPawn
                   && promotion.Board == Board;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(To, Piece, PromotedPawn, Board).GetHashCode();
        }
    }
}