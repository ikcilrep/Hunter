using System.Linq;
using System.Collections.Generic;
using System;
using Chess.Moves;

namespace Chess.Pieces
{
    public class Rook : Piece
    {
        public Rook(bool color)
        {
            _color = color;
            _weight = 5;
        }

        public static bool IsMovePossibleStatic(IMove move)
        {
            var position = board.FindPiece(move.Piece);
            var columnDistance = move.To.Column - position.Column;
            var rowDistance = move.To.Row - position.Row;
            var movesStraight = columnDistance == 0 ^ rowDistance == 0;
            if (!movesStraight)
            {
                return false;
            }
            var capturedPieces = 0;
            if (board.IsTherePieceOfColor(move.To, !move.Piece.Color))
            {
                capturedPieces = 1;
            }
            return board.PiecesInRange(move.To, position).Count - 1 == capturedPieces;

        }

        public override bool IsMovePossible(IMove move)
        {
            return IsMovePossibleStatic(move);
        }

        private static void AddRange(HashSet<Position> range,
                                     Piece piece,
                                     Board board,
                                     Position border,
                                     Func<Position, bool> predicate,
                                     HashSet<IMove> moves)
        {

            foreach (var beforeNotAllowedColumn in range.Where(predicate))
            {
                moves.Add(new Move(piece, beforeNotAllowedColumn, board, false));
            }
            if (board.IsTherePieceOfColor(border, !piece.Color))
            {
                moves.Add(new Move(piece, border, board, true));
            }
        }

        public static HashSet<IMove> PossibleMovesStatic(Piece piece, Board board)
        {
            var position = board.FindPiece(piece);
            var maxPosition1 = new Position(position.Row, Position.MaxColumn);
            var maxPosition2 = new Position(position.Row, Position.MinColumn);
            var maxPosition3 = new Position(Position.MaxRow, position.Column);
            var maxPosition4 = new Position(Position.MinRow, position.Column);

            var range1 = Positions.Range(position, maxPosition1);
            range1.Remove(position);
            var range2 = Positions.Range(position, maxPosition2);
            range2.Remove(position);
            var range3 = Positions.Range(position, maxPosition3);
            range3.Remove(position);
            var range4 = Positions.Range(position, maxPosition4);
            range4.Remove(position);

            var minNotAllowedColumn = board.PiecesInRange(range1)
                                           .OrderBy(c => c.Column)
                                           .First();
            var maxNotAllowedColumn = board.PiecesInRange(range2)
                                           .OrderBy(c => c.Column)
                                           .Last();
            var minNotAllowedRow = board.PiecesInRange(range3)
                                        .OrderBy(c => c.Row)
                                        .First();
            var maxNotAllowedRow = board.PiecesInRange(range4)
                                        .OrderBy(c => c.Row)
                                        .First();

            var result = new HashSet<IMove>();

            AddRange(range1,
                     piece,
                     board,
                     minNotAllowedColumn,
                     p => p.Column < minNotAllowedColumn.Column,
                     result);
            AddRange(range2,
                     piece,
                     board,
                     maxNotAllowedColumn,
                     p => p.Column > maxNotAllowedColumn.Column,
                     result);
            AddRange(range3,
                     piece,
                     board,
                     minNotAllowedRow,
                     p => p.Row < minNotAllowedColumn.Row,
                     result);
            AddRange(range4,
                     piece,
                     board,
                     maxNotAllowedRow,
                     p => p.Row > maxNotAllowedColumn.Row,
                     result);

            return result;

        }

        public override HashSet<IMove> PossibleMoves(Board board)
        {
            return PossibleMovesStatic(this, board);
        }

        public override string ToString()
        {
            return "R";
        }

    }
}