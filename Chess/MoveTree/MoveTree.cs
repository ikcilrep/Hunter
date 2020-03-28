using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Chess;
using Chess.Moves;
using Chess.Pieces;

namespace Player
{
    public class MoveTree
    {
        private HashSet<Node> _nodes = new HashSet<Node>();

        private Dictionary<BigInteger, MoveTree> _calculated;

        private int _materialSituation;

        public double MaterialSituation(int depth)
        {
            if (depth == 0 || _nodes.Count == 0)
            {
                return _materialSituation;
            }
            return _nodes.Average(n => n.Tree.MaterialSituation(depth - 1));
        }

        private void MakeTree(int depth, Board board)
        {
            var possibleMoves = board.PossibleMoves();
            foreach (var move in possibleMoves)
            {
                board.MakeMove(move);
                MoveTree tree;
                var serial = board.SerialNumber;

                if (_calculated.ContainsKey(serial))
                {
                    tree = _calculated[serial];
                    if (tree._nodes.Count> 0 && tree._nodes.First().Move.Piece.Color == move.Piece.Color) {
                        throw new Exception();
                    }
                }
                else
                {
                    tree = new MoveTree(depth - 1, board, _calculated);
                    _calculated[serial] = tree;
                }

                var node = new Node(move, tree);
                _nodes.Add(node);

                board.UndoLastMove();
            }
        }

        public MoveTree(int depth, Board board, Dictionary<BigInteger, MoveTree> calculated)
        {
            _calculated = calculated;
            if (depth > 0)
            {
                MakeTree(depth, board);
            }

            if (_nodes.Count == 0)
            {
                if (depth > 0)
                {
                    if (board.CurrentMoveColor == Board.White)
                    {
                        _materialSituation = -1000;
                    }
                    else
                    {
                        _materialSituation = 1000;
                    }
                }
                else
                {

                    _materialSituation = board.MaterialSituation;
                }
            }
        }


        public void Extend(int depth, Board board)
        {
            if (depth == 0 || _nodes.Count == 0)
            {
                if (_nodes.Count == 0)
                {
                    MakeTree(1, board);
                }
            }
            else
            {
                foreach (var node in _nodes)
                {
                    board.MakeMove(node.Move);
                    node.Tree.Extend(depth - 1, board);
                    board.UndoLastMove();
                }
            }
        }

        public IMove BestMove(int depth)
        {
            if (_nodes.Count == 0)
            {
                throw new InvalidOperationException();
            }
            var firstNode = _nodes.First();
            var bestMove = firstNode.Move;
            var bestMaterialSituation = firstNode.Tree.MaterialSituation(depth - 1);
            foreach (var node in _nodes.Skip(1))
            {
                var materialSituation = node.Tree.MaterialSituation(depth - 1);
                if (materialSituation > bestMaterialSituation)
                {
                    bestMaterialSituation = materialSituation;
                    bestMove = node.Move;
                }
            }
            return bestMove;
        }

        public MoveTree this[IMove move]
        {
            get
            {
                return _nodes.First(n => n.Move.Equals(move)).Tree;
            }
        }
    }
}