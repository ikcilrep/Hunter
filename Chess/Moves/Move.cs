using Chess.Pieces;

namespace Chess.Moves
{
    public class Move : IMove
    {

        public Piece Piece { get; }
        public Position To { get; }
        public bool IsCapture { get; }
        public Board Board { get; }

        public Move(Piece piece, Position to, Board board)
        {
            Piece = piece;
            To = to;
            IsCapture = board.IsTherePieceOfColor(To, !Piece.Color);
            Board = board;
        }

        public Move(Piece piece, Position to, Board board, bool isCapture)
        {
            Piece = piece;
            To = to;
            IsCapture = isCapture;
            Board = board;
        }

    }
}
