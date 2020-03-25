using System;
using System.Linq;
using Chess.Moves;
using Player;

namespace Chess

{
    class Program
    {
        private static IMove ReadMove(Board board)
        {
            var moveString = Console.ReadLine();
            try
            {
                return MoveParser.ParseMove(moveString, board);
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is ArgumentException)
                {
                    return null;
                }
                throw;
            }


        }
        static void Main(string[] args)
        {
            Board board = new Board();
            MoveTree moveTree = new MoveTree(2, board);
            var i = 1;
            while (true)
            {
                var bestMove = moveTree.BestMove;
                Console.WriteLine($"{i}.\t{bestMove}");
                board.MakeMove(bestMove);


                moveTree = moveTree[bestMove];
                moveTree.Extend(board);

                IMove move = null;
                while (move == null)
                {
                    Console.Write("\t");
                    move = ReadMove(board);
                    if (move == null)
                    {
                        Console.WriteLine("Incorrect move, enter correct one.");
                    }
                }

                board.MakeMove(move);

                moveTree = moveTree[move];
                moveTree.Extend(board);

                i++;
            }
        }
    }
}
