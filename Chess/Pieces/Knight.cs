using System;
using System.Collections.Generic;

namespace Chess

{
    public class Knight : Piece
    {
        public Knight(bool color) {
            _weight = 3;
            _color = color;
        }
        public override bool IsMovePossible(Move move, Board board)
        {
            var position = board.FindPiece(this);
            var columnDistance = Math.Abs(position.Column - move.To.Column);
            var rowDistance = Math.Abs(position.Row - move.To.Row);
            var distancesAreRight = (columnDistance == 2 && rowDistance == 1)
               || (columnDistance == 1 && rowDistance == 2);

            return !board.IsTherePieceOfColor(move.To, Color) && distancesAreRight;
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            var position = board.FindPiece(this);

            var result = new HashSet<Move>();

            void AddMove (int distance1, int distance2) 
            {
                try
                {
                    var newPosition = new Position((byte)(position.Row + distance1),
                                                   (byte)(position.Column + distance2));

                    if (!board.IsTherePieceOfColor(newPosition, Color))
                    {
                        result.Add(new Move(this, newPosition, board));
                    }
                }
                catch { }
            } 

            foreach (var distance1 in new int[]{ -2, 2}) {
                foreach (var distance2 in new int[] { -1, 1 })
                {
                        AddMove(distance1, distance2);
                        AddMove(distance2, distance1);
                }
            }                

           return result;
        }
    }
}