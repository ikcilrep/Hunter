using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Moves;
using Chess.Pieces;

namespace Chess
{
    public class Board
    {
        public const bool White = true;
        public const bool Black = false;

        private readonly Dictionary<Position, Piece> _startPositions = new Dictionary<Position, Piece> {
            { new Position("a1"), new Rook(White)},
            { new Position("b1"), new Knight(White)},
            { new Position("c1"), new Bishop(White)},
            { new Position("d1"), new Queen(White)},
            { new Position("e1"), new King(White)},
            { new Position("f1"), new Bishop(White)},
            { new Position("g1"), new Knight(White)},
            { new Position("h1"), new Rook(White)},
            { new Position("a2"), new Pawn(White)},
            { new Position("b2"), new Pawn(White)},
            { new Position("c2"), new Pawn(White)},
            { new Position("d2"), new Pawn(White)},
            { new Position("e2"), new Pawn(White)},
            { new Position("f2"), new Pawn(White)},
            { new Position("g2"), new Pawn(White)},
            { new Position("h2"), new Pawn(White)},
            { new Position("a8"), new Rook(Black)},
            { new Position("b8"), new Knight(Black)},
            { new Position("c8"), new Bishop(Black)},
            { new Position("d8"), new Queen(Black)},
            { new Position("e8"), new King(Black)},
            { new Position("f8"), new Bishop(Black)},
            { new Position("g8"), new Knight(Black)},
            { new Position("h8"), new Rook(Black)},
            { new Position("a7"), new Pawn(Black)},
            { new Position("b7"), new Pawn(Black)},
            { new Position("c7"), new Pawn(Black)},
            { new Position("d7"), new Pawn(Black)},
            { new Position("e7"), new Pawn(Black)},
            { new Position("f7"), new Pawn(Black)},
            { new Position("g7"), new Pawn(Black)},
            { new Position("h7"), new Pawn(Black)}
      };
        public int MaterialSituation
        {
            get
            {
                var whites = Pieces.Values.Where(p => p.Color == White).Sum(p => p.Weight);
                var blacks = Pieces.Values.Where(p => p.Color == Black).Sum(p => p.Weight);
                return whites - blacks;
            }
        }

        public Piece this[string positionString]
        {
            get
            {
                return Pieces[new Position(positionString)];
            }
        }

        public bool CurrentMoveColor
        {
            get => _currentMoveColor;
        }
        private List<IMove> _moves = new List<IMove>();
        private bool _currentMoveColor = White;

        private IEnumerable<IMove> PossibleMovesOfPiece(Piece piece)
        {
            return piece.PossibleMoves(this).Where(m => !IsCheckedAfterMove(m));
        }
        public IEnumerable<IMove> PossibleMoves(Func<Piece, bool> predicate)
        {
            return Pieces.Values.ToList().Where(predicate).SelectMany(PossibleMovesOfPiece);
        }

        public IEnumerable<IMove> PossibleMoves() => PossibleMoves(p => p.Color == CurrentMoveColor);

        public IMove LastMove { get => _moves.Last(); }
        public Dictionary<Position, Piece> Pieces { get; } = new Dictionary<Position, Piece>();

        public Dictionary<Position, Piece> StartPositions => _startPositions;

        public Board()
        {
            Pieces = new Dictionary<Position, Piece>(StartPositions);
        }


        public Position FindPiece(Piece piece) => Pieces.First(kvp => kvp.Value == piece).Key;

        public bool IsTherePieceOfColor(Position position, bool color)
        {
            return Pieces.ContainsKey(position) && Pieces[position].Color == color;
        }

        public bool IsTherePiece(Position position) => Pieces.ContainsKey(position);

        public HashSet<Position> PiecesInRange(HashSet<Position> range, bool color)
        {
            return range.Where(p => IsTherePieceOfColor(p, color)).ToHashSet();
        }

        public bool HasPieceBeenMoved(Piece piece) => _moves.Any(m => m.Piece == piece);

        public HashSet<Position> PiecesInRange(HashSet<Position> range)
        {

            return range
                .Where(p => IsTherePiece(p))
                .ToHashSet();
        }

        public HashSet<Position> PiecesInRange(Position position1, Position position2)
        {

            return Positions.Range(position1, position2)
                           .Where(p => IsTherePiece(p))
                           .ToHashSet();
        }
        public Position LastMoveFrom
        {
            get
            {
                var movesExceptLast = _moves.SkipLast(1);
                try
                {
                    return movesExceptLast.Last(m => m.Piece == LastMove.Piece).To;
                }
                catch (InvalidOperationException)
                {
                    return StartPositions.First(kvp => kvp.Value == LastMove.Piece).Key;
                }
            }
        }

        public void UndoLastMove()
        {
            if (_moves.Count == 0)
            {
                throw new InvalidOperationException("No move has been made yet!");
            }
            var movesExceptLast = _moves.SkipLast(1);
            _currentMoveColor = !_currentMoveColor;
            if (IMove.IsCapture(LastMove))
            {

                var capturedPiecePosition = LastMove.To;
                if (LastMove is EnPassant)
                {
                    capturedPiecePosition = capturedPiecePosition.Behind(LastMove.Piece.Color);
                }
                Piece capturedPiece = null;

                try
                {
                    capturedPiece = movesExceptLast.Where(m => m.To == capturedPiecePosition).Last().Piece;
                }
                catch (InvalidOperationException)
                {
                    capturedPiece = StartPositions[capturedPiecePosition];
                }

                if (LastMove is EnPassant)
                {
                    Pieces[LastMove.To.Behind(LastMove.Piece.Color)] = capturedPiece;
                }
                else
                {
                    Pieces[LastMove.To] = capturedPiece;
                }
            }
            else
            {
                Pieces.Remove(LastMove.To);
                if (LastMove is Castling)
                {
                    var castling = (Castling)LastMove;
                    Pieces[castling.RookPosition] = castling.Rook;
                    Pieces.Remove(castling.RookTo);
                }
            }

            Pieces[LastMoveFrom] = LastMove.Piece;
            _moves = movesExceptLast.ToList();
        }

        public void MakeMove(IMove move)
        {
            if (move is Castling)
            {
                MakeMove((Castling)move);
            }
            else if (move is EnPassant)
            {
                MakeMove((EnPassant)move);
            }
            else if (move is Move)
            {
                MakeMove((Move)move);
            }
            else if (move is Promotion)
            {
                MakeMove((Promotion)move);
            }
        }
        public void MakeMove(Move move) => Move(move);

        public void MakeMove(Promotion promotion)
        {
            var from = FindPiece(promotion.Pawn);
            _moves.Add(promotion);
            Move(promotion.To, from, promotion.PromotedPawn);
        }

        public void MakeMove(EnPassant enPassant)
        {
            Pieces.Remove(enPassant.To.Behind(enPassant.Piece.Color));
            Move(enPassant);
        }

        public void MakeMove(Castling castling)
        {
            Move(castling.RookTo, castling.RookPosition, castling.Rook);
            Move(castling);
        }

        private void Move(Position to, Position from, Piece piece)
        {
            if (piece.Color != _currentMoveColor)
            {
                throw new InvalidOperationException("It's not your turn!");
            }
            Pieces[to] = piece;
            Pieces.Remove(from);
            _currentMoveColor = !_currentMoveColor;
        }

        private void Move(Position to, Piece piece)
        {
            var from = FindPiece(piece);
            Move(to, from, piece);
        }

        private void Move(IMove move)
        {
            _moves.Add(move);
            Move(move.To, move.Piece);
        }

        public bool IsCheckedAfterMove(IMove move)
        {
            MakeMove(move);
            var isChecked = IsChecked();
            UndoLastMove();
            if (move is Castling castling)
            {
                var middleOfCastling = new Move(castling.King, castling.To.GoInDirectionOf(castling.RookPosition.Column, 1), this);
                MakeMove(middleOfCastling);
                isChecked |= IsChecked();
                UndoLastMove();
            }
            return isChecked;
        }

        private bool IsChecked()
        {
            return Pieces.Values.Any(p => p.Color != CurrentMoveColor && p.PossibleCaptures(this).Any(m => Pieces[m.To] is King));
        }
    }
}
