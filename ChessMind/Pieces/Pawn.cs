using System;
using System.Collections.Generic;

namespace ChessMind
{
    public class Pawn : Piece
    {
        private static bool DoesPawnMoveTwoForward(Move move, Board board) {
            var position = board.FindPiece(move.Piece);
            return move.Piece is Pawn && move.To.Row == position.Forward(2, move.Piece.Color);
        }
        public override bool IsMovePossible(Move move, Board board)
        {
            if (board.IsTherePieceOfColor(move.To, Color)) {
                return false;
            }
            var position = board.FindPiece(this);
            var startRow = Position.GetPawnRow(Color);
            var columnDistance = Math.Abs(move.To.Column - position.Column);
            var columnIsTheSame = columnDistance == 0; 
            var movesOneForward = move.To.Row == position.Forward(1, Color);
            var movesTwoForward = move.To.Row == position.Forward(2, Color);
            var hasntMovedYet = move.To.Row == startRow;
            if (columnIsTheSame) {
                return !board.IsTherePieceOfColor(move.To, !Color)
                       && (movesOneForward || (hasntMovedYet && movesTwoForward));
            }

            var movesDiagonally = movesOneForward
                                  && columnDistance == 1;
            if (!movesDiagonally) {
                return false;
            }

            var isNormalCapture = board.IsTherePieceOfColor(move.To, !Color);
            if (isNormalCapture) {
                return true;
            }

                var isEnPassant = DoesPawnMoveTwoForward(board.LastMove, board)
                                  && move.To.Row == Position.Forward(startRow, 3, Color) 
                                  && board.IsTherePieceOfColor(new Position(
                                      move.To.Forward(1, !Color),
                                      move.To.Column), !Color);
                return isEnPassant;

                                          
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            throw new System.NotImplementedException();
        }
    }
}