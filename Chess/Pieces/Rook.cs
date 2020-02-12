using System.Linq;
using System.Collections.Generic;
using System;

namespace Chess

{
    public class Rook : Piece
    {
        public Rook(bool color) {
            _color = color;
            _weight = 5;
        }
        public override bool IsMovePossible(Move move, Board board)
        {
            var position = board.FindPiece(this);
            var columnDistance = move.To.Column - position.Column;
            var rowDistance = move.To.Row - position.Row;
            var movesStraight = columnDistance == 0 ^ rowDistance == 0;
            if (!movesStraight)
            {
                return false;
            }
            var capturedPieces = 0;
            if (board.IsTherePieceOfColor(move.To, !Color)) 
            {
                capturedPieces = 1;
            }
           return board.PiecesInRange(move.To, position).Count -1 == capturedPieces;
        }

        private void AddMove(byte row, byte column, HashSet<Move> moves, Board board)
        {
                var position = new Position(row, column);
                moves.Add(new Move(this, position, board));
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            var position = board.FindPiece(this);
            var maxPosition1 = new Position(position.Row, Position.MaxColumn);
            var maxPosition2 = new Position(position.Row, Position.MinColumn);
            var maxPosition3 = new Position(Position.MaxRow, position.Column);
            var maxPosition4 = new Position(Position.MinRow, position.Column);

            var range1 = Position.Range(position, maxPosition1);
            range1.Remove(position);
            var range2 = Position.Range(position, maxPosition2);
            range2.Remove(position);
            var range3 = Position.Range(position, maxPosition3);
            range3.Remove(position);
            var range4 = Position.Range(position, maxPosition4);
            range4.Remove(position);

            var notAllowedFields1 = board.PiecesInRange(range1);
            var notAllowedFields2 = board.PiecesInRange(range2);
            var notAllowedFields3 = board.PiecesInRange(range3);
            var notAllowedFields4 = board.PiecesInRange(range4);

            var minNotAllowedColumn = notAllowedFields1.OrderBy(c => c.Column).First();
            var maxNotAllowedColumn = notAllowedFields2.OrderBy(c => c.Column).Last();
            var minNotAllowedRow = notAllowedFields3.OrderBy(c => c.Row).First();
            var maxNotAllowedRow = notAllowedFields4.OrderBy(c => c.Row).First(); 

            var result = new HashSet<Move>();

            foreach (var beforeNotAllowedColumn in range1.Where(p => p.Column < minNotAllowedColumn.Column)) 
            {
                result.Add(new Move(this, beforeNotAllowedColumn, board));
            }
                
            if (board.IsTherePieceOfColor(minNotAllowedColumn, !Color))
            {
                result.Add(new Move(this, minNotAllowedColumn, board));
            }

            foreach (var beforeNotAllowedColumn in range2.Where(p => p.Column > maxNotAllowedColumn.Column)) 
            {
                result.Add(new Move(this, beforeNotAllowedColumn, board));
            }
                
            if (board.IsTherePieceOfColor(maxNotAllowedColumn, !Color))
            {
                result.Add(new Move(this, minNotAllowedColumn, board));
            }


            foreach (var beforeNotAllowedRow in range3.Where(p => p.Row < minNotAllowedRow.Row)) 
            {
                result.Add(new Move(this, beforeNotAllowedRow, board));
            }
                
            if (board.IsTherePieceOfColor(minNotAllowedRow, !Color))
            {
                result.Add(new Move(this, minNotAllowedRow, board));
            }


            foreach (var beforeNotAllowedRow in range4.Where(p => p.Row > maxNotAllowedRow.Row)) 
            {
                result.Add(new Move(this, beforeNotAllowedRow, board));
            }
                
            if (board.IsTherePieceOfColor(maxNotAllowedRow, !Color))
            {
                result.Add(new Move(this, maxNotAllowedRow, board));
            }

            return result;
        }
    }
}