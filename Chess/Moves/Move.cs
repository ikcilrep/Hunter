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
            var piecePosition = Board.FindPiece(Piece);
            var movesToTheSamePlace = Board.PossibleMoves().Where(m => m.To == To
                    && m.Piece.Color == Piece.Color
                    && m.Piece.GetType() == Piece.GetType()).ToList();
            if (movesToTheSamePlace.Count == 2)
            {
                var otherPiecePosition = Board.FindPiece(movesToTheSamePlace[1].Piece);
                if (movesToTheSamePlace[1].To.Row == To.Row)
                {
                    result += piecePosition.ToString()[0];
                }
                else if (movesToTheSamePlace[1].To.Column == To.Column)
                {
                    result += piecePosition.ToString()[1];
                }
                else
                {
                    result += piecePosition.ToString()[0];
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
