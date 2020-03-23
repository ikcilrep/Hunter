using System;
using System.Linq;
using Chess.Moves;
using Chess.Pieces;
using NUnit.Framework;

namespace Chess.Tests
{
    [TestFixture]
    public class PawnTests
    {
        private static Board _board;

        [SetUp]
        public void Init()
        {
            _board = new Board();
            _board.MakeMove(new Move(_board["e2"], new Position("e4"), _board));
            _board.MakeMove(new Move(_board["d7"], new Position("d5"), _board));
            _board.MakeMove(new Move(_board["e4"], new Position("e5"), _board));
            _board.MakeMove(new Move(_board["a7"], new Position("a6"), _board));
            _board.MakeMove(new Move(_board["h2"], new Position("h4"), _board));
            _board.MakeMove(new Move(_board["a6"], new Position("a5"), _board));
            _board.MakeMove(new Move(_board["h4"], new Position("h5"), _board));
            _board.MakeMove(new Move(_board["a5"], new Position("a4"), _board));
            _board.MakeMove(new Move(_board["h5"], new Position("h6"), _board));
            _board.MakeMove(new Move(_board["a4"], new Position("a3"), _board));
            _board.MakeMove(new Move(_board["h6"], new Position("g7"), _board));
            _board.MakeMove(new Move(_board["f7"], new Position("f5"), _board));
        }

        [TestCase("g7", new string[] { "gxf8=B", "gxf8=N", "gxf8=Q", "gxf8=R", "gxh8=B", "gxh8=N", "gxh8=Q", "gxh8=R" })]
        [TestCase("b2", new string[] { "bxa3", "b3","b4" })]
        [TestCase("e5", new string[] { "exf6", "e6" })]
        public void PossibleMoves_ReturnCorrectMoves(string pawnPositionsString, string[] expectedPossibleMoves)
        {
            var pawn = (Pawn)_board[pawnPositionsString];
            var possibleMoves = pawn.PossibleMoves(_board).Select(m => m.ToString()).ToHashSet();
            var expectedPossibleMovesSet = expectedPossibleMoves.ToHashSet();
            Assert.That(possibleMoves.SetEquals(expectedPossibleMovesSet));
        }

        [TestCase("e5", "exf6")]
        [TestCase("b2", "b4")]
        [TestCase("g7", "gxf8=B")]
        public void IsMovePossible_PossibleMove_ReturnTrue(string pawnPositionsString, string moveString)
        {
            var knight = (Pawn)_board[pawnPositionsString];
            var move = MoveParser.ParseMove(moveString, _board);
            Assert.IsTrue(knight.IsMovePossible(move));
        }

    }
}