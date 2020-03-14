using System;
using System.Linq;
using Chess.Moves;

namespace Chess

{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Board();
            try
            {
                foreach (var piece in board.Pieces.Values)
                {

                    var possibleMoves = piece.PossibleMoves(board);

                    Console.WriteLine($"Possible moves {possibleMoves.Count}");


                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Hello World!");
        }
    }
}
