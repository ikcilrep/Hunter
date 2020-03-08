using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Moves;

namespace Chess.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(bool color)
        {
            _color = color;
            _weight = 3;
        }


        public override bool IsMovePossible(IMove move)
        {
            return IsMovePossibleStatic(move);
        }

        public static bool IsMovePossibleStatic(IMove move)
        {
            try
            {
                var position = move.Board.FindPiece(move.Piece);
                var diagonal = Positions.Diagonal(position, move.To);

                foreach (var diagonalPosition in diagonal.SkipLast(1))
                {
                    if (move.Board.IsTherePiece(diagonalPosition))
                    {
                        return false;
                    }
                }
                return !move.Board.IsTherePieceOfColor(move.To, move.Piece.Color);
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private static void AddDiagonalsEnds(Position position, HashSet<Position> diagonalsEnds)
        {
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

        public static HashSet<IMove> PossibleMovesStatic(Piece piece, Board board)
        {
            var position = board.FindPiece(piece);
            var result = new HashSet<IMove>();
            var diagonalsEnds = new HashSet<Position>();
            AddDiagonalsEnds(position, diagonalsEnds);
            foreach (var diagonalEnd in diagonalsEnds)
            {
                var diagonal = Positions.Diagonal(position, diagonalEnd);
                var diagonalFirstEmptyPositions = diagonal.TakeWhile(p => !board.IsTherePiece(p))
                    .Select(p => new Move(piece, p, board, false));
                result.UnionWith(diagonalFirstEmptyPositions);
                if (diagonal.Count > diagonalFirstEmptyPositions.Count())
                {
                    var capturePosition = diagonal[diagonalFirstEmptyPositions.Count()];
                    if (board.IsTherePieceOfColor(capturePosition, !piece.Color))
                    {
                        result.Add(new Move(piece, capturePosition, board, true));
                    }
                }
            }

            return result;
        }

        public override HashSet<IMove> PossibleMoves(Board board)
        {
            return PossibleMovesStatic(this, board);
        }

        public override string ToString()
        {
            return "B";
        }
    }
}