using System.Linq;
using Chess.Moves;
using Chess.Pieces;
using NUnit.Framework;

namespace Chess.Tests
{
    [TestFixture]
    public class KingTests
    {
        private static Board _board;

        [SetUp]
        public void Init()
        {
            _board = new Board();
            _board.MakeMove(new Move(_board["e2"], new Position("e4"), _board));
            _board.MakeMove(new Move(_board["e7"], new Position("e5"), _board));
            _board.MakeMove(new Move(_board["g1"], new Position("f3"), _board));
            _board.MakeMove(new Move(_board["b8"], new Position("c6"), _board));
            _board.MakeMove(new Move(_board["f1"], new Position("b5"), _board));
            _board.MakeMove(new Move(_board["a7"], new Position("a6"), _board));
        }

        [TestCase("e1", new string[] { "Kf1", "Ke2", "O-O" })]
        public void PossibleMoves_ReturnCorrectMoves(string kingPositionString, string[] expectedPossibleMoves)
        {
            var king = (King)_board[kingPositionString];
            var possibleMoves = king.PossibleMoves(_board).Select(m => m.ToString()).ToHashSet();
            var expectedPossibleMovesSet = expectedPossibleMoves.ToHashSet();
            Assert.That(possibleMoves.SetEquals(expectedPossibleMovesSet));
        }


        [TestCase("e1", "O-O")]
        [TestCase("e1", "Ke2")]

        public void IsMovePossible_PossibleMove_ReturnTrue(string kingPositionString, string moveString)
        {
            var king = (King)_board[kingPositionString];
            var move = MoveParser.ParseMove(moveString, _board);
            Assert.IsTrue(king.IsMovePossible(move));
        }

    }
}