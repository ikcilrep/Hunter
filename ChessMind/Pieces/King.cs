using System;
using System.Collections.Generic;

namespace ChessMind
{
    public class King : Piece
    {
        public King(bool color) {
            _color = color;
            _weight = 40; 
        }
        public override bool IsMovePossible(Move move, Board board)
        {
            var position = board.FindPiece(this);
            var rowDistance = Math.Abs(position.Row - move.To.Row);
            var columnDistance = Math.Abs(position.Column - move.To.Column);
            var distancesAreRight = rowDistance
                   < 2 && columnDistance < 2;
            var thereIsAlliedPiece = board.IsTherePiece(position) && board[position].Color == Color;
            return  distancesAreRight && !thereIsAlliedPiece && move.To != position;     
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            var position = board.FindPiece(this);
            var result = new HashSet<Move>();
            for (byte rowDistance = 0; rowDistance < 2; rowDistance++) { 
                for (byte columnDistance = 0; columnDistance < 2; columnDistance++) {
                    Position newPosition = null;
                    try
                    {
                        newPosition = new Position((byte)(rowDistance + position.Row),
                                                       (byte)(columnDistance + position.Column));
                    }
                    finally
                    {
                        Move move = new Move(newPosition, board);
                        if (IsMovePossible(move, board))
                        {
                            result.Add(move);
                        }
                    }
                }
            }

            return result;
        }
    }
}