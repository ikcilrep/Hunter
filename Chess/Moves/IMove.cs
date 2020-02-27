using System;
using Chess.Pieces;
using System.Linq;

namespace Chess.Moves
{
    public interface IMove
    {
        public Piece Piece { get; }
        public Position To { get; }
        public Board Board { get; }

        public static bool IsCapture(IMove move) => (move is Promotion && ((Promotion)move).IsCapture)
            || (move is Move && ((Move)move).IsCapture)
            || move is EnPassant;

        public static IMove ParseMove(string moveString, Board board)
        {
            var isShortCastling = moveString.Equals("O-O");
            var isLongCastling = moveString.Equals("O-O-O");
            if (isShortCastling || isLongCastling)
            {
                var king = (King)board.Pieces.Values.Where(p => p.Color == !board.LastMove.Piece.Color && p is King).First();
                if (Castling.IsCastling(king, isLongCastling, board))
                {
                    return new Castling(king, isLongCastling, board);
                }
            }
            throw new NotImplementedException();
        }

    }
}
