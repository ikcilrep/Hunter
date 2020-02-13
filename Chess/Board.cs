using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess
{
    public class Board
    {
        public static bool White { get => true; }
        public static bool Black { get => false; }

        
        private readonly Dictionary<Position, Piece> _startPieces; 
        private List<Move> _moves = new List<Move>();

        public HashSet<Move> PossibleMoves 
        {
             get => Pieces.Values.SelectMany(p => p.PossibleMoves(this)).ToHashSet();
        }

        public HashSet<Move> PossibleCaptures 
        {
             get => Pieces.Values.SelectMany(p => p.PossibleCaptures(this)).ToHashSet();
        }

        public Move LastMove { get => _moves.Last(); }
        public Dictionary<Position, Piece> Pieces { get; } = new Dictionary<Position, Piece>();

        public Board()
        {
            
            var players = new Tuple<bool, byte>[2] {
                new Tuple<bool, byte>(White, Position.MinRow),
                new Tuple<bool, byte>(Black, Position.MaxRow) };
            foreach (var player in players)
            {
                var pawnRow = Positions.GetPawnRow(player.Item1);
                for (byte column = Position.MinColumn; column <= Position.MaxColumn; column++)
                {
                    var pawnPosition = new Position(pawnRow, column);
                    Pieces.Add(pawnPosition, new Pawn(player.Item1));
                }

                var rookPosition1 = new Position(player.Item2, 0);
                var rook1 = new Rook(player.Item1);
                Pieces.Add(rookPosition1, rook1);

                var rookPosition2 = new Position(player.Item2, 7);
                var rook2 = new Rook(player.Item1);
                Pieces.Add(rookPosition2, rook2);

                var knightPosition1 = new Position(player.Item2, 1);
                var knight1 = new Knight(player.Item1);
                Pieces.Add(knightPosition1, knight1);

                var knightPosition2 = new Position(player.Item2, 6);
                var knight2 = new Knight(player.Item1);
                Pieces.Add(knightPosition2, knight2);

                var bishopPosition1 = new Position(player.Item2, 2);
                var bishop1 = new Bishop(player.Item1);
                Pieces.Add(bishopPosition1, bishop1);

                var bishopPosition2 = new Position(player.Item2, 5);
                var bishop2 = new Bishop(player.Item1);
                Pieces.Add(bishopPosition2, bishop2);

                var queenPosition = new Position(player.Item2, 3);
                var queen = new Queen(player.Item1);
                Pieces.Add(queenPosition, queen);

                var kingPosition = new Position(player.Item2, 3);
                var king = new King(player.Item1);
                Pieces.Add(kingPosition, king);
            }
            _startPieces = new Dictionary<Position, Piece>(Pieces);
        }


        public Position FindPiece(Piece piece) => Pieces.First(kvp => kvp.Value == piece).Key;

        public bool IsTherePieceOfColor(Position position, bool color)
        {
            return Pieces.ContainsKey(position) && Pieces[position].Color == color;
        }

        public bool IsTherePiece(Position position)
        {
            return Pieces.ContainsKey(position);
        }

        public HashSet<Position> PiecesInRange(HashSet<Position> range, bool color)
        {
            return range.Where(p => IsTherePieceOfColor(p, color)).ToHashSet();
        }

        public bool HasPieceBeenMoved(Piece piece)
        {
            return _moves.Where(m => m.Piece == piece).Count() > 0;
        }

        public HashSet<Position> PiecesInRange(HashSet<Position> range)
        {

            return range
                .Where(p => IsTherePiece(p))
                .ToHashSet();
        }

        public HashSet<Position> PiecesInRange(Position position1, Position position2)
        {

            return Positions.Range(position1, position1)
                           .Where(p => IsTherePiece(p))
                           .ToHashSet();
        }

        public void UndoLastMove()
        {
            var movesWithoutLast = _moves.SkipLast(1);
            if (LastMove.IsCapture)
            {

                var capturedPiecePosition = LastMove.To;
                if (LastMove.IsEnPassant) {
                    capturedPiecePosition = capturedPiecePosition.Behind(LastMove.Piece.Color);
                }
                Piece capturedPiece = null;

                try
                {
                    capturedPiece = movesWithoutLast.Where(m => m.To == capturedPiecePosition).Last().Piece;
                } catch (InvalidOperationException) {
                    capturedPiece = _startPieces[capturedPiecePosition];
                } 
                Pieces[LastMove.To] = capturedPiece;
            }
            else
            {
                Pieces.Remove(LastMove.To);
            }
            var from = movesWithoutLast.Where(m => m.Piece == LastMove.Piece).Last().To;
            Pieces[from] = LastMove.Piece;
            _moves = movesWithoutLast.ToList();
        }

        public void MakeMove(Move move)
        {           
            if (move.IsEnPassant) {
                Pieces.Remove(move.To.Behind(move.Piece.Color));
            }
            Pieces[move.To] = move.Piece;
            Pieces.Remove(FindPiece(move.Piece));
            _moves.Add(move);
            if (IsChecked(move.Piece.Color))
            {
                UndoLastMove();
                throw new System.Exception("Check after move.");
            }
            
        }

        public bool IsChecked(bool color) {
            foreach (var move in PossibleCaptures)
            {
                if (move.Piece.Color != color && Pieces[move.To] is King)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
