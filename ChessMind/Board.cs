using System.Collections.Generic;
using System.Linq;

namespace ChessMind
{
    public class Board
    {
        private Dictionary<Position, Piece> _pieces;

        public bool IsTherePieceOfColor(Position position, bool color)
        {
            return _pieces.ContainsKey(position) && _pieces[position].Color == color;
        }

        public Position FindPiece(Piece piece) => _pieces.First(kvp => kvp.Value == piece).Key;



        public HashSet<Position> PiecesInRanges(HashSet<Position> range, bool color)
        {
            return range.Where(p => IsTherePieceOfColor(p, color)).ToHashSet();
        }

        public Piece this[Position position] => _pieces[position];

        private Position JustDoMove(Move move)
        {
            _pieces[move.To] = move.Piece;
            var from = FindPiece(move.Piece);
            _pieces.Remove(from);
            return from;
        }

        public void MakeMove(Move move)
        {

            Piece capturedPiece = null;
            if (move.IsCapture)
            {
                capturedPiece = _pieces[move.To];
            }

            var from = JustDoMove(move);

            if (IsChecked(move.Piece.Color))
            {
                _pieces[from] = move.Piece;
                if (move.IsCapture) {
                    _pieces[move.To] = capturedPiece;
                }
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