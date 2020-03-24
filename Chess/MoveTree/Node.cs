using Chess.Moves;

namespace Player
{
    public class Node
    {
        public IMove Move { get; }
        public MoveTree Tree { get; }

        public Node(IMove move, MoveTree tree)
        {
            Move = move;
            Tree = tree;
        }
    }
}