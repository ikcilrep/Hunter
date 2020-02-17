using System;
using System.Collections.Generic;
using Chess.Moves;

namespace Chess.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(bool color)
        {
            _color = color;
            _weight = 1;
        }


        public override bool IsMovePossible(IMove move, Board board)
        {

            if (move is EnPassant)
            {
                return true;
            }

            if (board.IsTherePieceOfColor(move.To, Color))
            {
                return false;
            }

            var position = board.FindPiece(this);
            var columnDistance = Math.Abs(move.To.Column - position.Column);
            var columnIsTheSame = columnDistance == 0;
            var movesOneForward = move.To.Row == position.Forward(1, Color);
            if (columnIsTheSame)
            {
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

        private bool AddMoveIfNotBlocked(Position position, Board board, HashSet<IMove> moves)
        {

            if (!board.IsTherePiece(position))
            {
                moves.Add(new Move(this, position, board));
                return true;
            }
            return false;
        }

        public override HashSet<IMove> PossibleMoves(Board board)
        {
            var position = board.FindPiece(this);
            var result = new HashSet<IMove>();
            var before = position.Before(Color);
            if (AddMoveIfNotBlocked(before, board, result)
                && !board.HasPieceBeenMoved(this))
            {
                AddMoveIfNotBlocked(before.Before(Color), board, result);
            }
            void AddCapture(int columnDistance)
            {
                try
                {
                    var to = new Position(before.Row,
                                              (byte)(position.Column + columnDistance));
                    if (board.IsTherePieceOfColor(to, !Color))
                    {
                        _ = result.Add(new Move(this, to, true));
                    }
                    else if (EnPassant.IsEnPassant(this, to, board))
                    {
                        result.Add(new EnPassant(this, to));
                    }
                }
                catch { }
            }

            AddCapture(+1);
            AddCapture(-1);

            return result;
        }
    }
}