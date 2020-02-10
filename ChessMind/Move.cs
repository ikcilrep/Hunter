using System;
namespace ChessMind
{
    public class Move
    {

        public Piece Piece { get; }
        public Position To { get; }
        public bool IsCapture { get; }

        public Move(Piece piece, Position to, Board board)
        {
            Piece = piece;
            To = to;
            IsCapture = board.IsTherePieceOfColor(To, !Piece.Color);
        }
    }
}
