using Chess.Pieces;

namespace Chess.Moves
{
    public class Promotion : IMove
    {
        public Piece Piece => Pawn;
        public Pawn Pawn { get; }

        public Position To { get; }

        public bool IsCapture { get; }

        public Promotion(Pawn pawn, Position to, Board board)
        {
            Pawn = pawn;
            To = to;
            IsCapture = board.IsTherePieceOfColor(To, !Piece.Color);
        } 
        
        public Promotion(Pawn pawn, Position to, bool isCapture)
        {
            Pawn = pawn;
            To = to;
            IsCapture = isCapture;
        } 


    }
}