using System.Collections.Generic;
using System.Linq;

namespace ChessMind
{
    public class Board
    {
        private readonly Dictionary<Position, Piece> _pieces = new Dictionary<Position, Piece>();
        private List<Move> _moves = new List<Move>();

        public Move LastMove { get => _moves.Last(); }

        public bool IsTherePieceOfColor(Position position, bool color)
        {
            return _pieces.ContainsKey(position) && _pieces[position].Color == color;
        }

        public bool IsTherePiece(Position position)
        {
            return _pieces.ContainsKey(position);
        }



        public Position FindPiece(Piece piece) => _pieces.First(kvp => kvp.Value == piece).Key;



        public HashSet<Position> PiecesInRange(HashSet<Position> range, bool color)
        {
            return range.Where(p => IsTherePieceOfColor(p, color)).ToHashSet();
        }

        public bool HasPieceBeenMoved(Piece piece)
        {
            return _moves.Where(m => m.Piece == piece).Count() > 0;
        }

        public HashSet<Position> PiecesInRange(Position position1, Position position2, bool color)
        {
            return PiecesInRange(Position.Range(position1, position2), color); 
        }


        public Piece this[Position position] => _pieces[position];

        public void UndoLastMove()
        {
            var lastMove = _moves.Last();
            if (lastMove.IsCapture)
            {
                var capturedPiece = _moves.SkipLast(1).Where(m => m.To == lastMove.To).Last().Piece;
                _pieces[lastMove.To] = capturedPiece;
            } else
            {
                _pieces.Remove(lastMove.To);
            }
            var from = _moves.SkipLast(1).Where(m => m.Piece == lastMove.Piece).Last().To;
            _pieces[from] = lastMove.Piece;
            _moves = _moves.SkipLast(1).ToList();
        }

        public void MakeMove(Move move)
        {           
            _pieces[move.To] = move.Piece;
            _pieces.Remove(FindPiece(move.Piece));
            _moves.Add(move);
            if (IsChecked(move.Piece.Color))
            {
                UndoLastMove();
                throw new System.Exception("Check after move.");
            }
            
        }

        public bool IsChecked(bool color) { 
            foreach (var piece in _pieces.Values) { 
                foreach (var move in piece.PossibleCaptures(this)) { 
                    if (move.Piece.Color != color && _pieces[move.To] is King) {
                        return true;
                    } 
                } 
            }
            return false;
        }
    }
}
