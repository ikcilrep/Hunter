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
            board.MakeMove(MoveParser.ParseMove("e4", board));
            board.MakeMove(MoveParser.ParseMove("d5", board));
            board.MakeMove(MoveParser.ParseMove("exd5", board));
            board.MakeMove(MoveParser.ParseMove("Qxd5", board));
            board.MakeMove(MoveParser.ParseMove("Nc3", board));
            board.MakeMove(MoveParser.ParseMove("Qd8", board));
            board.MakeMove(MoveParser.ParseMove("Ke2", board));
            board.MakeMove(MoveParser.ParseMove("Nc6", board));
            board.MakeMove(MoveParser.ParseMove("Nb5", board));
            board.MakeMove(MoveParser.ParseMove("Nf6", board));
            board.MakeMove(MoveParser.ParseMove("Nxa7", board));
            board.MakeMove(MoveParser.ParseMove("Rxa7", board));
            board.MakeMove(MoveParser.ParseMove("Nf3", board));
            board.MakeMove(MoveParser.ParseMove("e5", board));
            board.MakeMove(MoveParser.ParseMove("Nxe5", board));


            MoveTree moveTree = new MoveTree(3, board);
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
