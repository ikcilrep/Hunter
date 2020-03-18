using System;
using Chess.Pieces;

namespace Chess.Moves
{
    public class EnPassant : IMove
    {
        public Piece Piece => Pawn;

        public Pawn Pawn { get; }

        public Position To { get; }
        public Board Board { get; }

        public EnPassant(Pawn pawn, Position to, Board board)
        {
            Pawn = pawn;
            To = to;
            Board = board;
        }

        public static bool IsEnPassant(Pawn pawn, Position to, Board board)
        {
            var startRow = Positions.GetPawnRow(pawn.Color);
            var position = board.FindPiece(pawn);
            var columnDistance = Math.Abs(to.Column - position.Column);
            var movesOneForward = to.Row == position.Forward(1, pawn.Color);
            var movesDiagonally = movesOneForward
                              && columnDistance == 1;
            return movesDiagonally
                              && !board.IsTherePieceOfColor(to, pawn.Color)
                              && HasPawnJustMovedTwoForward(board)
                              && position.Row == Positions.Forward(startRow, 3, pawn.Color)
                              && board.IsTherePieceOfColor(to.Behind(pawn.Color),
                                                           !pawn.Color);


        }

        private static bool HasPawnJustMovedTwoForward(Board board)
        {
            try
            {
                Console.WriteLine(board.LastMoveFrom);
                return board.LastMove.Piece is Pawn && board.LastMove.To.Row == board.LastMoveFrom.Forward(2, board.LastMove.Piece.Color);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Board.FindPiece(Pawn).ToString()[0] + "x" + To;
        }

        public override bool Equals(object obj)
        {
            return obj is EnPassant enPassant && enPassant.Pawn == Pawn && enPassant.To == To && enPassant.Board == Board;
        }

        public override int GetHashCode() {
            return Tuple.Create(To, Pawn, Board).GetHashCode();
        }
    }
}
