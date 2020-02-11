using System;
using System.Collections.Generic;

namespace Chess
{
    public class Pawn : Piece
    {
        private static bool DoesPawnMoveTwoForward(Move move, Board board) {
            var position = board.FindPiece(move.Piece);
            return move.Piece is Pawn && move.To.Row == position.Forward(2, move.Piece.Color);
        }

        internal static bool IsEnPassant(Move move, Board board) {
            if (move.Piece is Pawn)
            {
                var startRow = Position.GetPawnRow(move.Piece.Color);
                var position = board.FindPiece(move.Piece);
                var columnDistance = Math.Abs(move.To.Column - position.Column);
                var movesOneForward = move.To.Row == position.Forward(1, move.Piece.Color);
                var movesDiagonally = movesOneForward
                                  && columnDistance == 1;
                return movesDiagonally 
                                  && !board.IsTherePieceOfColor(move.To, move.Piece.Color)
                                  && DoesPawnMoveTwoForward(board.LastMove, board)
                                  && position.Row == Position.Forward(startRow, 3, move.Piece.Color) 
                                  && board.IsTherePieceOfColor(move.To.Behind(move.Piece.Color),
                                                               !move.Piece.Color);


            }
            return false;
        }

        public override bool IsMovePossible(Move move, Board board)
        {
            if (move.IsEnPassant) 
            {
                return true;
            }

            if (board.IsTherePieceOfColor(move.To, Color)) {
                return false;
            }

            var position = board.FindPiece(this);
            var columnDistance = Math.Abs(move.To.Column - position.Column);
            var columnIsTheSame = columnDistance == 0; 
            var movesOneForward = move.To.Row == position.Forward(1, Color);
            if (columnIsTheSame) {
                var movesTwoForward = move.To.Row == position.Forward(2, Color);
                var pieceHasBeenMoved = board.HasPieceBeenMoved(this);
                var thereIsPieceOnTheWay = board.IsTherePiece(position.Before(Color));
                return !board.IsTherePieceOfColor(move.To, !Color)
                       && (movesOneForward || (!pieceHasBeenMoved
                                               && movesTwoForward
                                               && !thereIsPieceOnTheWay
                                               ));
            }

            var movesDiagonally = movesOneForward
                                  && columnDistance == 1;

            return movesDiagonally && board.IsTherePieceOfColor(move.To, !Color);
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}