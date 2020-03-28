using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Chess.Moves;
using Player;
using System.Threading;

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
            /* board.MakeMove(MoveParser.ParseMove("Na3", board));
            board.MakeMove(MoveParser.ParseMove("Nc6", board));
            board.MakeMove(MoveParser.ParseMove("Rb1", board));
            board.MakeMove(MoveParser.ParseMove("b6", board));
            board.MakeMove(MoveParser.ParseMove("Nb5", board));
            board.MakeMove(MoveParser.ParseMove("Ba6", board));
            board.MakeMove(MoveParser.ParseMove("Nxa7", board));
            board.MakeMove(MoveParser.ParseMove("Nxa7", board));
            board.MakeMove(MoveParser.ParseMove("Nf3", board));
            board.MakeMove(MoveParser.ParseMove("d5", board));
            board.MakeMove(MoveParser.ParseMove("Rg1", board));
            board.MakeMove(MoveParser.ParseMove("Qd6", board));
            board.MakeMove(MoveParser.ParseMove("h3", board));
            board.MakeMove(MoveParser.ParseMove("O-O-O", board));
            board.MakeMove(MoveParser.ParseMove("Nh4", board));
            board.MakeMove(MoveParser.ParseMove("Re8", board));

 */
            Thread T = new Thread(() =>
            {
                var calculated = new Dictionary<BigInteger, MoveTree>();
                int depth = 4;
                MoveTree moveTree = new MoveTree(depth, board, calculated);
                var i = 1;
                while (true)
                {
                    var bestMove = moveTree.BestMove(depth);
                    Console.WriteLine($"{i}.\t{bestMove}");
                    board.MakeMove(bestMove);


                    moveTree = moveTree[bestMove];
                    moveTree.Extend(depth - 1, board);

                    IMove move = null;
                    while (move == null || board.IsCheckedAfterMove(move))
                    {
                        Console.Write("\t");
                        move = ReadMove(board);
                        if (move == null || board.IsCheckedAfterMove(move))
                        {
                            Console.WriteLine("Incorrect move, enter correct one.");
                        }
                    }

                    board.MakeMove(move);

                    moveTree = moveTree[move];
                    moveTree.Extend(depth - 1, board);

                    i++;
                }

            }, 2097152);
            T.Start();
        }
    }
}
