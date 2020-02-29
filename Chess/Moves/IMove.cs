using Chess.Pieces;

namespace Chess.Moves
{
    public interface IMove
    {
        public Piece Piece { get; }
        public Position To { get; }
        public Board Board { get; }

        public static bool IsCapture(IMove imove) => (imove is Promotion promotion && promotion.IsCapture)
            || (imove is Move move && move.IsCapture)
            || imove is EnPassant;

    }
}
