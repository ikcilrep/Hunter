using System;
using Chess.Pieces;

namespace Chess.Moves
{
    public class EnPassant : IMove
    {
        public Piece Piece => Pawn;

        public Pawn Pawn { get;  }

        public Position To { get; }

        public EnPassant(Pawn pawn, Position to)
        {
            Pawn = pawn;
            To = to;
        }

        public static bool IsEnPassant(Pawn pawn, Position to, Board board) {
                var startRow = Positions.GetPawnRow(pawn.Color);
                var position = board.FindPiece(pawn);
                var columnDistance = Math.Abs(to.Column - position.Column);
                var movesOneForward = to.Row == position.Forward(1, pawn.Color);
                var movesDiagonally = movesOneForward
                                  && columnDistance == 1;
                return movesDiagonally 
                                  && !board.IsTherePieceOfColor(to, pawn.Color)
                                  && DoesPawnMoveTwoForward(board.LastMove, board)
                                  && position.Row == Positions.Forward(startRow, 3, pawn.Color) 
                                  && board.IsTherePieceOfColor(to.Behind(pawn.Color),
                                                               !pawn.Color);


        }

        private static bool DoesPawnMoveTwoForward(IMove move, Board board) {
            var position = board.FindPiece(move.Piece);
            return move.Piece is Pawn && move.To.Row == position.Forward(2, move.Piece.Color);
        }

    }
}
