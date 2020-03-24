namespace Player
{
    public class Node
    {
        IMove Move { get; }
        MoveTree Tree { get; }

        public Node(IMove move, MoveTree tree)
        {
            Move = move;
            Tree = tree;
        }
    }
}