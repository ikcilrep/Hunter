using System;
namespace ChessMind
{
    public class Move
    {
        public Piece Piece { get; }
        public Position To { get; }
        public bool IsCapture { get; }
        public bool IsCheck { get; }

        public Move()
        {
        }
    }
}
