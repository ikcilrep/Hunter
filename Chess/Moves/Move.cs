using Chess.Pieces;
using System;
using System.Linq;

namespace Chess.Moves
{
    public class Move : IMove
    {

        public Piece Piece { get; }
        public Position To { get; }
        public bool IsCapture { get; }
        public Board Board { get; }

        public Move(Piece piece, Position to, Board board)
        {
            Piece = piece;
            To = to;
            IsCapture = board.IsTherePieceOfColor(To, !Piece.Color);
            Board = board;
        }

        public Move(Piece piece, Position to, Board board, bool isCapture)
        {
            Piece = piece;
            To = to;
            IsCapture = isCapture;
            Board = board;
        }

        public override string ToString()
        {
            var result = Piece.ToString();
            if (Piece is Pawn && IsCapture)
            {
                result += Board.FindPiece(Piece).ToString()[0];
            }
            var piecePosition = Board.FindPiece(Piece);
            var movesToTheSamePlace = Board.PossibleMoves(p => p != Piece && p.GetType() == Piece.GetType() && p.Color == Piece.Color).Where(m => m.To == To);
            if (movesToTheSamePlace.Count() == 1)
            {
                var move = movesToTheSamePlace.First();
                var otherPiecePosition = Board.FindPiece(move.Piece);
                if (move.To.Row == To.Row || move.To.Column != To.Column)
                {
                    result += piecePosition.ToString()[0];
                }
                else 
                {
                    result += piecePosition.ToString()[1];
                }
            }
            if (IsCapture)
            {
                result += "x";
            }
            result += To;
            return result;
        }

        public override bool Equals(object obj)
        {
            return obj is Move move && move.Piece == Piece && move.To == To && move.Board == Board;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(To, Piece, Board).GetHashCode();
        }
    }
}
