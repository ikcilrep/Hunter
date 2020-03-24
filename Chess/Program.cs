using System;
using System.Linq;
using Chess.Moves;
using Player;

namespace Chess

{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            MoveTree moveTree = new MoveTree(10, board);
            var i = 1;
            while (true)
            {
                Console.WriteLine($"{i}. {moveTree.BestMove}");

                var move = MoveParser.ParseMove(Console.ReadLine(), board);
                board.MakeMove(move);
                moveTree.Extend(board);
                i++;
            }
        }
    }
}
