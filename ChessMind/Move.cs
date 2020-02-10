using System;
namespace ChessMind
{
    public class Move
    {
        private object position;
        private Board board;

        public Piece Piece { get; }
        public Position To { get; }
        public bool IsCapture { get; }
        public bool IsCheck { get; }

        public Move()
        {
        }

        public Move(object position, Board board)
        {
            this.position = position;
            this.board = board;
        }
    }
}
