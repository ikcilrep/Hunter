using Chess.Pieces;

namespace Chess.Moves
{
    public class Move : IMove
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
        
        public Move(Piece piece, Position to, bool isCapture)
        {
            Piece = piece;
            To = to;
            IsCapture = isCapture;
        } 

    }
}
