using System.Linq;
using System.Collections.Generic;

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
           return board.PiecesInRange(move.To, position, Color).Count > 0;
        }

        public override HashSet<Move> PossibleMoves(Board board)
        {
            var position = board.FindPiece(this);
            var maxPosition1 = new Position(position.Row, Position.MaxColumn);
            var maxPosition2 = new Position(position.Row, Position.MinColumn);
            var maxPosition3 = new Position(Position.MaxRow, position.Column);
            var maxPosition4 = new Position(Position.MinRow, position.Column);

            var notAllowedFields1 = board.PiecesInRange(position, maxPosition1, Color);
            var notAllowedFields2 = board.PiecesInRange(position, maxPosition2, Color);
            var notAllowedFields3 = board.PiecesInRange(position, maxPosition3, Color);
            var notAllowedFields4 = board.PiecesInRange(position, maxPosition4, Color);

            var minNotAllowedColumn = notAllowedFields1.Min(c => c.Column);
            var maxNotAllowedColumn = notAllowedFields2.Max(c => c.Column);
            var minNotAllowedRow = notAllowedFields3.Min(c => c.Row);
            var maxNotAllowedRow = notAllowedFields4.Max(c => c.Row);

            var result = new HashSet<Move>();

            for (var column = position.Column; column > minNotAllowedColumn; column--) {
                var newPosition = new Position(position.Row, column);
                result.Add(new Move(this, position, board));
            }
 

            for (var column = position.Column; column < maxNotAllowedColumn; column++) {
                var newPosition = new Position(position.Row, column);
                result.Add(new Move(this, position, board));
            }

            for (var row = position.Row; row > minNotAllowedRow; row--) {
                var newPosition = new Position(row, position.Column);
                result.Add(new Move(this, position, board));
            }
 

            for (var row = position.Row; row < maxNotAllowedRow; row++) {
                var newPosition = new Position(row, position.Column);
                result.Add(new Move(this, position, board));
            }
            return result;
        }
    }
}