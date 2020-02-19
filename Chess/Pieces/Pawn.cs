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

            if (move is EnPassant || move is Promotion)
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

        private bool AddMoveIfNotBlocked(Position to, Board board, HashSet<IMove> moves)
        {
            if (!board.IsTherePiece(to))
            {
                if (to.Row == Position.MaxRow || to.Row == Position.MinRow)
                {
                    moves.Add(new Promotion(this, new Bishop(Color), to, board));
                    moves.Add(new Promotion(this, new Knight(Color), to, board));
                    moves.Add(new Promotion(this, new Queen(Color), to, board));
                    moves.Add(new Promotion(this, new Rook(Color), to, board));
                }
                else
                {
                    moves.Add(new Move(this, to, board));
                }
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
                        _ = result.Add(new Move(this, to, board, true));
                    }
                    else if (EnPassant.IsEnPassant(this, to, board))
                    {
                        result.Add(new EnPassant(this, to, board));
                    }
                }
                catch { }
            }

            AddCapture(+1);
            AddCapture(-1);

            return result;
        }
        public override string ToString()
        {
            return "";
        }
    }
}