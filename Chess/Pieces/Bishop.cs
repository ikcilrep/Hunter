using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    public class Bishop : Piece
    {
        public override bool IsMovePossible(Move move, Board board)
        {
            try 
            {
                var position = board.FindPiece(this);
                var diagonal = Positions.Diagonal(position, move.To);

                foreach (var diagonalPosition in diagonal) { 
                    if (board.IsTherePiece(diagonalPosition)) 
                    {
                        return false;
                    }
                }
                return true;
            } 
            catch 
            {
                return false;
            }
        }

        private static void AddDiagonalsEnds(Position position, HashSet<Position> diagonalsEnds) {
            var distance1 = Math.Min(
                Position.MaxRow - position.Row,
                Position.MaxColumn - position.Column);
            var distance2 = Math.Min(
                position.Row - Position.MinRow,
                position.Column - Position.MinColumn);
            var distance3 = Math.Min(
                Position.MaxRow - position.Row,
                position.Column - Position.MinColumn);
            var distance4 = Math.Min(
                position.Row - Position.MinRow,
                Position.MaxColumn - position.Column);

            diagonalsEnds.Add(new Position(
                (byte)(position.Row + distance1),
                (byte)(position.Column + distance1)));
            diagonalsEnds.Add(new Position(
                (byte)(position.Row - distance2),
                (byte)(position.Column - distance2)));
            diagonalsEnds.Add(new Position(
                (byte)(position.Row + distance3),
                (byte)(position.Column - distance3)));
            diagonalsEnds.Add(new Position(
                (byte)(position.Row - distance4),
                (byte)(position.Column + distance4)));
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            var position = board.FindPiece(this);
            var result = new HashSet<Move>();
            var diagonalsEnds = new HashSet<Position>();
            AddDiagonalsEnds(position, diagonalsEnds);
            foreach (var diagonalEnd in diagonalsEnds) 
            {
                var diagonal = Positions.Diagonal(position, diagonalEnd);
                var diagonalFirstEmptyPositions = diagonal.TakeWhile(p => !board.IsTherePiece(p))
                    .Select(p => new Move(this, p, false));
                result.UnionWith(diagonalFirstEmptyPositions);
                if (diagonal.Count > diagonalFirstEmptyPositions.Count())
                {
                    var capturePosition = diagonal[diagonalFirstEmptyPositions.Count()];
                    if (board.IsTherePieceOfColor(capturePosition, !Color))
                    {
                        result.Add(new Move(this, capturePosition, true));
                    }
                }
            }

            return result;
        }
    }
}