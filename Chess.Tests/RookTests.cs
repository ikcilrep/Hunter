using System;
using System.Linq;
using Chess.Moves;
using Chess.Pieces;
using NUnit.Framework;

namespace Chess.Tests
{
    [TestFixture]
    public class RookTests
    {
        private static Board _board;

        [SetUp]
        public void Init()
        {
            _board = new Board();
            _board.MakeMove(new Move(_board["a2"], new Position("a4"), _board));
            _board.MakeMove(new Move(_board["e7"], new Position("e5"), _board));
            _board.MakeMove(new Move(_board["a1"], new Position("a3"), _board));
            _board.MakeMove(new Move(_board["d7"], new Position("d5"), _board));
            _board.MakeMove(new Move(_board["a3"], new Position("e3"), _board));
            _board.MakeMove(new Move(_board["c7"], new Position("c5"), _board));
        }

        [TestCase("e3", new string[] { "Re4", "Rxe5", "Ra3", "Rb3", "Rc3", "Rd3", "Rf3", "Rg3", "Rh3" })]
        public void PossibleMoves_ReturnCorrectMoves(string rookPositionString, string[] expectedPossibleMoves)
        {
            var rook = (Rook)_board[rookPositionString];
            var possibleMoves = rook.PossibleMoves(_board).Select(m => m.ToString()).ToHashSet();
            Console.WriteLine(String.Join(",", possibleMoves));
            var expectedPossibleMovesSet = expectedPossibleMoves.ToHashSet();
            Assert.That(possibleMoves.SetEquals(expectedPossibleMovesSet));
        }

        [TestCase("e3", "Re4")]
        [TestCase("e3", "Rxe5")]
        [TestCase("e3", "Ra3")]
        public void IsMovePossible_PossibleMove_ReturnTrue(string rookPositionString, string moveString)
        {
            var rook = (Rook)_board[rookPositionString];
            var move = MoveParser.ParseMove(moveString, _board);
            Assert.IsTrue(rook.IsMovePossible(move));
        }

    }
}