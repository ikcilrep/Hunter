using System.Collections.Generic;
using System.Linq;
using Chess;

namespace Player
{
    public class MoveTree
    {
        private HashSet<Node> _nodes = new HashSet<Node>();

        private int _materialSituation;

        public double MaterialSituation
        {
            get
            {
                if (_nodes.Count == 0)
                {
                    return _materialSituation;
                }
                return _nodes.Average(n => n.Tree.MaterialSituation);
            }
        }
        public MoveTree(int depth, Board board)
        {
            if (depth > 0)
            {
                var possibleMoves = board.PossibleMoves();
                foreach (var move in possibleMoves)
                {
                    board.MakeMove(move);
                    var node = new Node(move, new MoveTree(depth - 1, board));
                    _nodes.Add(node);
                    board.UndoLastMove();
                }
            }
            else
            {
                _materialSituation = board.MaterialSituation;
            }
        }

        public void Extend(Board board)
        {
            if (_nodes.Count == 0)
            {
                var possibleMoves = board.PossibleMoves();
                foreach (var move in possibleMoves)
                {
                    var node = new Node(move, new MoveTree(0, board));
                    _nodes.Add(node);
                }
            }
            else
            {
                foreach (var node in _nodes)
                {
                    board.MakeMove(node.Move);
                    node.Tree.Extend(board);
                    board.UndoLastMove();
                }
            }
        }





    }
}