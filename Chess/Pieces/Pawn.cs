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
            Init();
        }


        public override bool IsMovePossible(IMove move)
        {

            if (move is EnPassant || move is Promotion)
            {
                return true;
            }

            if (move.Board.IsTherePieceOfColor(move.To, Color))
            {
                return false;
            }

            var position = move.Board.FindPiece(this);
            var columnDistance = Math.Abs(move.To.Column - position.Column);
            var columnIsTheSame = columnDistance == 0;
            var movesOneForward = move.To.Row == position.Forward(1, Color);
            if (columnIsTheSame)
            {
                var movesTwoForward = move.To.Row == position.Forward(2, Color);
                var pieceHasBeenMoved = move.Board.HasPieceBeenMoved(this);
                var thereIsPieceOnTheWay = move.Board.IsTherePiece(position.Before(Color));
                return !move.Board.IsTherePieceOfColor(move.To, !Color)
                       && (movesOneForward || (!pieceHasBeenMoved
                                               && movesTwoForward
                                               && !thereIsPieceOnTheWay
                                               ));
            }

            var movesDiagonally = movesOneForward
                                  && columnDistance == 1;

            return movesDiagonally && move.Board.IsTherePieceOfColor(move.To, !Color);
        }

        private bool AddMoveIfNotBlocked(Position to, Board board, HashSet<IMove> moves)
        {
            if (!board.IsTherePiece(to))
            {
                AddMoveOrPromotions(to, board, moves);
                return true;
            }
            return false;
        }

        private void AddMoveOrPromotions(Position to, Board board, HashSet<IMove> moves)
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
        }

        public override HashSet<IMove> PossibleMoves(Board board)
        {
            var position = board.FindPiece(this);
            var result = new HashSet<IMove>();
            try
            {
                var before = position.Before(Color);
                if (AddMoveIfNotBlocked(before, board, result)
                    && !board.HasPieceBeenMoved(this))
                {
                    try
                    {
                        AddMoveIfNotBlocked(before.Before(Color), board, result);
                    }
                    catch (ArgumentException) { }
                }


                void AddCapture(int columnDistance)
                {
                    try
                    {
                        var to = new Position(before.Row,
                                                  (byte)(position.Column + columnDistance));
                        if (board.IsTherePieceOfColor(to, !Color))
                        {
                            AddMoveOrPromotions(to, board, result);
                        }
                        else if (EnPassant.IsEnPassant(this, to, board))
                        {
                            result.Add(new EnPassant(this, to, board));
                        }
                    }
                    catch (ArgumentException) { }
                }

                AddCapture(+1);
                AddCapture(-1);
            }
            catch (ArgumentException) { }
            return result;
        }
        public override string ToString()
        {
            return "";
        }
    }
}