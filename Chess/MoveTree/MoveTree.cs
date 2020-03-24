using System.Collections.Generic;
using Chess;

namespace Player
{
    public class MoveTree
    {
        private HashSet<Node> _nodes = new HashSet<Node>();
        public int Depth { get; set; } = 0;

        public MoveTree(int depth, Board board)
        {
            Depth = depth << 1;
            if (Depth > 0)
            {
                var possibleMoves = board.PossibleMoves();
                foreach (var move in possibleMoves)
                {
                    board.MakeMove(move);
                    _nodes.Add(new Node(move, new MoveTree(depth - 1, board)));
                    board.UndoLastMove();
                }
            }
        }


    }
}